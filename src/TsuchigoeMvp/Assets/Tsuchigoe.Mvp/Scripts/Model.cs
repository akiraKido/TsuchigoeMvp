namespace Tsuchigoe.Mvp
{
    public abstract class Model : RestrictedMonoBehaviour
    {
        protected sealed override void Start()
        {
        }

        protected sealed override void Awake()
        {
        }

        public abstract void Initialize();

        public abstract void AfterBind();
    }
}