namespace untitledplantgame.Item;

public interface ICombinable<T> where T: ICombinable<T> {
	T Combine(T other);
}
