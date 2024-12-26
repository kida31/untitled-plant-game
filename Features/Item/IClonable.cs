using untitledplantgame.Item.Components;

namespace untitledplantgame.Item;

public interface IClonable<T>
	where T : IClonable<T>
{
	T Clone();
}
