using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApi.Data.DataBase;

/// <summary>
/// Конвенция для автоматического добавления триггеров к таблицам, если они ещё не заданы.
/// Используется для автоматического добавления триггеров в модели базы данных в Entity Framework.
/// </summary>
public class BlankTriggerAddingConvention : IModelFinalizingConvention {

    /// <summary>
    /// Процесс добавления триггеров к таблицам модели при финализации модели.
    /// Проверяет каждую сущность и добавляет триггер, если он ещё не задан.
    /// </summary>
    /// <param name="modelBuilder">Объект, который строит модель базы данных.</param>
    /// <param name="context">Контекст для обработки финализации модели.</param>
    public virtual void ProcessModelFinalizing(
        IConventionModelBuilder modelBuilder,
        IConventionContext<IConventionModelBuilder> context) {

        // Проходим по всем сущностям модели
        foreach (var entityType in modelBuilder.Metadata.GetEntityTypes()) {

            // Получаем имя таблицы для текущей сущности
            var table = StoreObjectIdentifier.Create(entityType, StoreObjectType.Table);

            // Если у сущности нет триггера и она не является дочерней сущностью
            if (table != null
                && entityType.GetDeclaredTriggers().All(t => t.GetDatabaseName(table.Value) == null)
                && (entityType.BaseType == null
                    || entityType.GetMappingStrategy() != RelationalAnnotationNames.TphMappingStrategy)) {
                // Добавляем триггер для таблицы
                entityType.Builder.HasTrigger(table.Value.Name + "_Trigger");
            }

            // Для фрагментов таблиц добавляем триггер, если его нет
            foreach (var fragment in entityType.GetMappingFragments(StoreObjectType.Table)) {
                if (entityType.GetDeclaredTriggers().All(t => t.GetDatabaseName(fragment.StoreObject) == null)) {
                    entityType.Builder.HasTrigger(fragment.StoreObject.Name + "_Trigger");
                }
            }
        }
    }
}
