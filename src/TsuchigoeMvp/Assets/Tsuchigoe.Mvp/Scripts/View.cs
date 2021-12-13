namespace Tsuchigoe.Mvp
{
    public abstract class View : RestrictedMonoBehaviour
    {
        protected sealed override void Start()
        {
        }

        protected override void Awake()
        {
        }
        
        public abstract void Initialize();

        public abstract void AfterBind();
    }
}