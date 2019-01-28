using RuhRoh.Triggers.Internal;
using System;

namespace RuhRoh.Triggers
{
    internal class CombinedTrigger : IUpdateableTrigger
    {
        public CombinedTrigger(Logical operand, ITrigger first) 
        {
            if (operand == Logical.Not)
            {
                // TODO move to resx
                throw new ArgumentException(nameof(operand), "The Not operand can not be used in this way.");
            }

            Operand = operand;
            First = first ?? throw new ArgumentNullException(nameof(first));
        }

        public CombinedTrigger()
        {
            Operand = Logical.Not;
        }

        public Logical Operand { get; }
        public ITrigger First { get; }
        public ITrigger Second { get; internal set; }

        public void Update()
        {
            if (First is IUpdateableTrigger updateableTrigger)
            {
                updateableTrigger.Update();
            }

            if (Second is IUpdateableTrigger updateableTrigger2)
            {
                updateableTrigger2.Update();
            }
        }

        public bool WillAffect()
        {
            if (Second == null)
            {
                // TODO Move to resx
                throw new InvalidOperationException("This combined trigger has not been fully configured.");
            }

            if (Operand == Logical.Not)
            {
                return !Second.WillAffect();
            }

            var first = First.WillAffect();
            switch (Operand)
            {
                case Logical.And:
                    return first && Second.WillAffect();
                case Logical.Or:
                    return first || Second.WillAffect();
                case Logical.Xor:
                    return first ^ Second.WillAffect();
            }

            return false;
        }
    }
}
