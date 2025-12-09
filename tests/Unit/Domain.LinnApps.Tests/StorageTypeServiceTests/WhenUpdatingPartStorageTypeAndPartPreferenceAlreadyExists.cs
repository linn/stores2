namespace Linn.Stores2.Domain.LinnApps.Tests.StorageTypeServiceTests
{
    using System;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingPartStorageTypeAndPartPreferenceAlreadyExists : ContextBase
    {
        private PartStorageType partsStorageType, alreadyExistPartStorageType;

        private Part part;

        private StorageType storageType;

        private PartStorageTypeException result;

        [SetUp]
        public void SetUp()
        {
            this.part = new Part { Id = 1, PartNumber = "Part No 1", Description = "Part 1" };

            this.storageType = new StorageType { StorageTypeCode = "Storage Type No 1", };

            this.partsStorageType = new PartStorageType(this.part, this.storageType, "a", 100, 50, "1", 1);

            this.alreadyExistPartStorageType = new PartStorageType(this.part, this.storageType, "a", 100, 50, "1", 2);

            this.PartsRepository.FindByIdAsync(Arg.Any<string>()).Returns(this.part);

            this.StorageTypeRepository.FindByIdAsync(Arg.Any<string>()).Returns(this.storageType);

            this.PartStorageTypeRepository.FindByAsync(Arg.Any<Expression<Func<PartStorageType, bool>>>())
                .Returns(this.partsStorageType);

            this.PartStorageTypeRepository.FindByAsync(Arg.Any<Expression<Func<PartStorageType, bool>>>())
                .Returns(this.alreadyExistPartStorageType);

            this.result = Assert.ThrowsAsync<PartStorageTypeException>(
                async () => await this.Sut.ValidatePartsStorageType(this.partsStorageType));
        }

        [Test]
        public void ShouldThrowException()
        {
            this.result.Should().BeOfType<PartStorageTypeException>();
        }
    }
}
