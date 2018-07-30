namespace MUtils.Cqrs {
    public interface IValidator<in TValidatable> {
        void ValidateObject(TValidatable instance);
    }
}