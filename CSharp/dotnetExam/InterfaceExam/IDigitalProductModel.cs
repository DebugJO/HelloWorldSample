namespace InterfaceExam
{
    public interface IDigitalProductModel : IProductModel
    {
        int TotalDownloadsLeft { get; }
    }
}
