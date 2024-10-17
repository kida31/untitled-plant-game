using System.Collections.Generic;

public interface IStorableCollection<T>: IList<T>
    where T: class, IStorable {

}