using System;
using System.Collections.Generic;
using System.Linq;

using Fx.Net.Monads.Option;
using Fx.Net.Monads.Option.Extensions;

var repo = new RepositoryMock();

var users = repo.GetAll()
    .Map(x => x.FirstOrDefault(x => x.Age > 25))
    .Map(x => x.Name)
    .Do(x => Console.WriteLine($"Значение: {x}"))
    .UnwrapOr("Имя отсуствует");



Console.WriteLine();

record User(string Name, int Age);


class RepositoryMock
{
    private readonly Dictionary<int, User?> _users = new Dictionary<int, User?>()
    {
        [0] = new User("Jack", 25), [1] = new User("Dima", 21), [2] = new User("Max", 17), [3] = new User("James", 35)
    };


    public Option<User> GetById(int id)
    {
        return _users.TryGetValue(id, out User? user)
            ? user.ToOption()
            : Option.None;
    }

    public Option<List<User>> GetAll()
    {
        List<User> users = [];

        for (int i = 0; i < _users.Count; ++i)
        {
            if (_users.TryGetValue(i, out User user))
            {
                if (user is not null) users.Add(user);
            }
        }

        return users.ToOption();
    }
}