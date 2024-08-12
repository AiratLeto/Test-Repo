using Xunit;

namespace Cars.Api.IntegrationTest
{
	/// <summary>
	/// Коллекция тестов, работающих в БД, и использующих ее разделяемый инстанс
	/// </summary>
	[CollectionDefinition(nameof(TestCollectionFixture))]
	public class TestCollectionFixture : ICollectionFixture<TestInitializerFixture>
	{
		// This class has no code, and is never created. Its purpose is simply
		// to be the place to apply [CollectionDefinition] and all the
		// ICollectionFixture<> interfaces.
	}
}
