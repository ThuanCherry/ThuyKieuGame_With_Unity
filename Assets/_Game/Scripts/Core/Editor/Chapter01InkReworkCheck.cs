using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ink.Runtime;
using UnityEditor;
using UnityEngine;

namespace ThuyKieu.Core.Editor
{
    public static class Chapter01InkReworkCheck
    {
        public static void Run()
        {
            ThuyKieu.Dialogue.Editor.InkStoryCompiler.Compile();
            string json = AssetDatabase.LoadAssetAtPath<TextAsset>(ThuyKieu.Dialogue.Editor.InkStoryCompiler.Output).text;
            var checks = new List<string>();
            var variants = new List<object>();
            var allText = new HashSet<string>();
            var allChoices = new List<string[]>();
            bool passed = true;
            Action<bool,string> check = (ok, label) => { passed &= ok; checks.Add((ok ? "PASS: " : "FAIL: ") + label); };
            string[] knots = {"c1_clue_debt","c1_clue_money","c1_clue_jewelry"};
            string[] flags = {"c1_hoi_lai_lich","c1_quan_sat_ma","c1_hoi_400","c1_tien_truoc"};
            for (int pair = 0; pair < 3; pair++)
            for (int probe = 0; probe < 4; probe++)
            for (int contract = 0; contract < 3; contract++)
            {
                var story = new Story(json);
                bool end = false;
                int choices = 0, iteration = 0;
                var gates = new HashSet<string>();
                while (++iteration < 600)
                {
                    if (story.canContinue)
                    {
                        string text = story.Continue().Trim();
                        if (text.Length > 0 && text != "@cue") allText.Add(text);
                        var tags = story.currentTags.ToArray();
                        foreach (var tag in tags)
                        {
                            if (tag.StartsWith("gate:")) gates.Add(tag.Substring(5));
                            if (tag == "chapter_end:1") end = true;
                        }
                        if (tags.Contains("gate:explore"))
                        {
                            int count = (int)story.variablesState["c1_clues"];
                            story.ChoosePathString(count < 2 ? knots[(pair + count) % 3] : "c1_before_knock");
                        }
                    }
                    else if (story.currentChoices.Count > 0)
                    {
                        allChoices.Add(story.currentChoices.Select(c=>c.text).ToArray());
                        int index = choices++ == 0 ? probe : contract;
                        story.ChooseChoiceIndex(index);
                    }
                    else break;
                }
                string label = "pair="+pair+" probe="+probe+" contract="+contract;
                bool ok = end && choices == 2 && (int)story.variablesState["c1_clues"] == 2
                    && (bool)story.variablesState[flags[probe]]
                    && (bool)story.variablesState["c1_co_tram_gia_dinh"]
                    && (bool)story.variablesState["c1_doc_ky_khe"] == (contract == 0)
                    && (bool)story.variablesState["c1_tien_truoc"] == (probe == 3 || contract == 1)
                    && (int)story.variablesState["tinh_tao"] == (probe < 2 ? 1 : 2) + (contract < 2 ? 1 : 0)
                    && (int)story.variablesState["tu_trong"] == (contract == 2 ? 1 : 2)
                    && gates.SetEquals(new[]{"hallway","mother","explore","door","offer","contract","waiting","exit"});
                check(ok, label+" preserves choices, stats, gates, hairpin and ending");
                variants.Add(new {pair,probe,contract,passed=ok,lines=iteration});
                var restored = new Story(json); restored.state.LoadJson(story.state.ToJson());
                restored.ChoosePathString("chapter_2");
                check((bool)restored.variablesState["c1_co_tram_gia_dinh"] && (int)restored.variablesState["tinh_tao"] == (int)story.variablesState["tinh_tao"], label+" survives Chapter 2 state restore");
            }
            var replay = new Story(json);
            for (int i=0;i<2;i++)
            {
                replay.ChoosePathString("c1_clue_debt");
                while (replay.canContinue) replay.Continue();
            }
            check((int)replay.variablesState["c1_clues"]==1, "Repeated clue knot cannot increment twice");
            replay.ChoosePathString("c1_clue_money"); while(replay.canContinue) replay.Continue();
            replay.ChoosePathString("c1_clue_jewelry"); while(replay.canContinue) replay.Continue();
            check((int)replay.variablesState["c1_clues"]==3, "Third clue stays optional and can still be inspected");
            File.WriteAllText("Tools/Chapter01/ReworkInkReport.json", Newtonsoft.Json.JsonConvert.SerializeObject(new {passed,checks,variants}, Newtonsoft.Json.Formatting.Indented));
            File.WriteAllText("Tools/Chapter01/ReworkDialogueCorpus.json", Newtonsoft.Json.JsonConvert.SerializeObject(new {lines=allText.OrderBy(t=>t).ToArray(), choices=allChoices.GroupBy(c=>string.Join("|",c)).Select(g=>g.First()).ToArray()}, Newtonsoft.Json.Formatting.Indented));
            if (!passed) throw new InvalidOperationException("Ink rework checks failed; inspect ReworkInkReport.json.");
        }
    }
}
