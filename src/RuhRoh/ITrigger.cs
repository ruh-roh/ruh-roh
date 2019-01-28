using RuhRoh.Affectors;

namespace RuhRoh
{
    /// <summary>
    /// Defines a trigger. A trigger can enable or disable the <see cref="IAffector"/> on which the trigger has been configured.
    /// </summary>
    public interface ITrigger
    {
        /// <summary>
        /// Returns <c>true</c> if this trigger should affect the current call.
        /// </summary>
        bool WillAffect();
    }

    /// <summary>
    /// Defines an updateable trigger.
    /// </summary>
    public interface IUpdateableTrigger : ITrigger
    {
        /// <summary>
        /// Update the state of the trigger when calling the affected method.
        /// Might change the result of a next call of <see cref="ITrigger.WillAffect"/>.
        /// </summary>
        void Update();
    }
}