using Idata.Entities.Core.Interfaces;
using Ihelpers.DataAnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Idata.Entities.Core
{
    /// <summary>
    /// EntityBase is the base class that holds common properties and methods of the entities.
    /// </summary>
    public abstract class EntityTranslationBase<TTranslation> : EntityBase
      where TTranslation : TranslationEntityBase
    {
        public ICollection<TTranslation> Translations { get; set; } = new List<TTranslation>();

        public TTranslation? GetTranslation(string language)
        {
            return Translations.FirstOrDefault(t => t.locale == language)
                ?? Translations.FirstOrDefault(); // fallback
        }
    }
}
