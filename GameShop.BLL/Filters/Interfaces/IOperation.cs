namespace GameShop.BLL.Filters.Interfaces
{
    public interface IOperation<T>
    {
        T Execute(T input);
    }
}
