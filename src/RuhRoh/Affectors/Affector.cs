using Castle.DynamicProxy;
using RuhRoh.Triggers;
using RuhRoh.Triggers.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RuhRoh.Affectors
{
    /// <summary>
    /// Base class for affectors
    /// </summary>
    public abstract class Affector : IAffector
    {
        private readonly List<ITrigger> _triggers = new List<ITrigger>();
        private CombinedTrigger _nextCombination;

        /// <summary>
        /// Combine the next trigger with the previous one using an "and" logical operator
        /// </summary>
        public Affector And
        {
            get
            {
                PrepareCombinedTrigger(Logical.And);
                return this;
            }
        }

        /// <summary>
        /// Combine the next trigger with the previous one using an "or" logical operator
        /// </summary>
        public Affector Or
        {
            get
            {
                PrepareCombinedTrigger(Logical.Or);
                return this;
            }
        }

        /// <summary>
        /// Combine the next trigger with the previous one using an "xor" logical operator
        /// </summary>
        public Affector Xor
        {
            get
            {
                PrepareCombinedTrigger(Logical.Xor);
                return this;
            }
        }

        /// <summary>
        /// Combine the next trigger using a "not" logical operator
        /// </summary>
        public Affector Not
        {
            get
            {
                var next = new CombinedTrigger();
                if (_nextCombination != null)
                {
                    _nextCombination.Second = next;
                    _nextCombination = next;
                    return this;
                }

                _nextCombination = next;
                _triggers.Add(_nextCombination);

                return this;
            }
        }

        ITrigger[] IAffector.Triggers => _triggers.ToArray();

        void IAffector.AddTrigger(ITrigger trigger)
        {
            if (_nextCombination != null)
            {
                _nextCombination.Second = trigger;
                _nextCombination = null;
                return;
            }

            _triggers.Add(trigger);
        }

        void IAffector.Affect(IInvocation invocation)
        {
            Affect(invocation);
        }

        bool IAffector.RunsBeforeMethodExecution => RunsBeforeMethodExecution;

        /// <summary>
        /// Execute the affector
        /// </summary>
        /// <param name="invocation">The affected method invocation. Only needed if the affector runs after the method is invoked for now.</param>
        protected internal abstract void Affect(IInvocation invocation);

        /// <summary>
        /// Indicates if the affector runs before the affected method is executed.
        /// Defaults to <c>true</c>.
        /// </summary>
        protected internal virtual bool RunsBeforeMethodExecution => true;

        private void PrepareCombinedTrigger(Logical operation)
        {
            if (_triggers.Count == 0 || _nextCombination != null)
            {
                throw new InvalidOperationException();
            }

            _nextCombination = new CombinedTrigger(operation, _triggers.Last());
            _triggers.RemoveAt(_triggers.Count - 1);
            _triggers.Add(_nextCombination);
        }
    }
}