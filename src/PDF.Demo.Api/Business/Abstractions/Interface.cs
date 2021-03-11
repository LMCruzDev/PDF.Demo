namespace PDF.Demo.Api.Business.Abstractions
{
    public interface IHtmlBuilder<TObject>
        where TObject : class
    {
        string Create(TObject form, TObject formData);
    }
}
