using Game;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CombatSpeedConfigurer
{
    class WrapAroundList : LinkedList<CombatSpeedSetting>
    {
        private LinkedListNode<CombatSpeedSetting> currentSpeedNode;

        public WrapAroundList(IEnumerable<CombatSpeedSetting> settings) : base(settings)
        {
            currentSpeedNode = this.First;
        }

        private bool AnyActivated
        {
            get
            {
                LinkedListNode<CombatSpeedSetting> current = this.First;
                do
                {
                    if (current.Value.IsActivated)
                        return true;
                    else
                        current = current.Next;
                } while (current != null);

                return false;
            }
        }

        public float Next()
        {
            // If no one is activated, then we don't bother going through the list
            if (!AnyActivated)
                return -1;

            do
            {
                if (currentSpeedNode == this.Last)
                    currentSpeedNode = this.First;
                else
                    currentSpeedNode = currentSpeedNode.Next;
            } while (!currentSpeedNode.Value.IsActivated);

            return currentSpeedNode.Value.CombatSpeed;
        }

        public float Previous()
        {
            // If no one is activated, then we don't bother going through the list
            if (!AnyActivated)
                return -1;

            do
            {
                if (currentSpeedNode == this.First)
                    currentSpeedNode = this.Last;
                else
                    currentSpeedNode = currentSpeedNode.Previous;
            } while (!currentSpeedNode.Value.IsActivated);

            return currentSpeedNode.Value.CombatSpeed;
        }
    }
}
