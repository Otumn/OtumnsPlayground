using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Playground
{
    public class GameManager : MonoBehaviour
    {
        public static GameState state = new GameState();
    }

    public class GameState
    {
        private List<Entity> entities = new List<Entity>();

        public void RegisterEntity(Entity ent)
        {
            entities.Add(ent);
        }

        public void UnRegisterEntity(Entity ent)
        {
            entities.Remove(ent);
        }

    }
}
