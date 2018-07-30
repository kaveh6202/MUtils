namespace MUtils.CqrsBase {
    public interface IValidator<in TValidatable> {
        void ValidateObject(TValidatable instance);
    }
}