namespace RuhRoh.Core
{
    public interface IAffector
    {
        void Affect();
        void AddTrigger(ITrigger trigger);

        ITrigger[] Triggers { get; }
    }
}