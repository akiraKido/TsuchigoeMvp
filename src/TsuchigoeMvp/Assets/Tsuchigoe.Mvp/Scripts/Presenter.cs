// ReSharper disable MemberCanBePrivate.Global
#nullable enable

namespace Tsuchigoe.Mvp
{
    public abstract class Presenter<TModel, TView> : RestrictedMonoBehaviour
        where TModel : Model
        where TView : View
    {
        protected TModel Model = null!;
        protected TView  View  = null!;
        
        protected sealed override void Start()
        {
            Model = GetComponent<TModel>();
            View  = GetComponent<TView>();

            SceneLoader.PresenterLoadComplete(Model);
            
            Model.Initialize();
            View.Initialize();
            
            BindModel();
            BindView();
            
            Model.AfterBind();
            View.AfterBind();
        }
        
        protected sealed override void Awake() { }

        protected abstract void BindModel();
        protected abstract void BindView();
    }
}