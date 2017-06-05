namespace RuhRoh.Core
{
    public interface ITrigger
    {
        bool WillAffect();
    }

    public interface IUpdatableTrigger
    {
        void Update();
    }
}