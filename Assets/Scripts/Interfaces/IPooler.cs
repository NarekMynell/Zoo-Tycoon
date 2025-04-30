public interface IPooler<T>
{
    public bool TryGetInstance(out T instance);

    public T ForceGetInstance();

    public void ReturnInstance(T instance);
}