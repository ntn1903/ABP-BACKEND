using Acme.BookStore.Books.Dtos;
using Acme.BookStore.Configurations.Dtos;
using Acme.BookStore.Entities;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Acme.BookStore;

public partial class BookStoreApplicationMappers
{
    /* You can configure your Mapperly mapping configuration here.
     * Alternatively, you can split your mapping configurations
     * into multiple mapper classes for a better organization. */
}

[Mapper]
public partial class BookToBookDtoMapper : MapperBase<Book, BookDto>
{
    public override partial BookDto Map(Book source);

    public override partial void Map(Book source, BookDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public partial class CreateUpdateBookDtoToBookMapper : MapperBase<CreateUpdateBookDto, Book>
{
    public override partial Book Map(CreateUpdateBookDto source);

    public override partial void Map(CreateUpdateBookDto source, Book destination);
}

// Configuration
[Mapper]
public partial class ConfigurationToConfigurationDtoMapper : MapperBase<Configuration, ConfigurationDto>
{
    public override partial ConfigurationDto Map(Configuration source);

    public override partial void Map(Configuration source, ConfigurationDto destination);
}