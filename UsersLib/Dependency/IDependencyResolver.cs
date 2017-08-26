using System.Collections.Generic;

namespace UsersLib.Dependency
{
    /// <summary>
    /// Интерфейс для использования DI без привязки к конкретному контейнеру
    /// </summary>
    internal interface IDependencyResolver
    {
        /// <summary>
        /// Предоставляет доступ к зарегистрированному сервису
        /// </summary>
        /// <returns>Возвращает сервис из IoC-контейнера. Если объект не найден, то возвращается null.</returns>
        T GetService<T>();
        /// <summary>
        /// Предоставляет доступ к всем зарегистрированным сервисам типа T
        /// </summary>
        /// <returns>Возвращает коллекцию сервисов заданного типа из IoC-контейнера. Если ничего найдено не было, то возвращается пустая коллекция.</returns>
        IEnumerable<T> GetServices<T>();
    }
}