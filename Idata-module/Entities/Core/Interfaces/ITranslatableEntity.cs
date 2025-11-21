namespace Idata.Entities.Core.Interfaces
{
    public interface ITranslatableEntity<TTranslation>
     where TTranslation : class, ITranslationEntity
    {
        ICollection<TTranslation> Translations { get; set; }

        TTranslation GetTranslation(string locale);
    }
}
