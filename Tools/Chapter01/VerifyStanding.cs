var player=UnityEngine.Object.FindAnyObjectByType<ThuyKieu.Player.PlayerMovement>();
return new {position=player.transform.position.ToString(), locked=player.ControlLocked, seated=player.GetComponentInChildren<Animator>().GetBool("IsSitting"), grounded=player.GetComponent<CharacterController>().isGrounded};
