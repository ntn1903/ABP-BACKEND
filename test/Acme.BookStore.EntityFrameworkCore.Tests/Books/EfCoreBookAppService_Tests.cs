using Acme.BookStore.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Acme.BookStore.Books
{
    [Collection(BookStoreTestConsts.CollectionDefinitionName)]
    public class EfCoreBookAppService_Tests : BookAppService_Tests<BookStoreEntityFrameworkCoreTestModule>
    {

    }
}
