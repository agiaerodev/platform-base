namespace Idata.Entities.Core
{
    public abstract class TranslationEntityBase : EntityBase
    {
        public string locale { get; set; } = null!;
    }
}
