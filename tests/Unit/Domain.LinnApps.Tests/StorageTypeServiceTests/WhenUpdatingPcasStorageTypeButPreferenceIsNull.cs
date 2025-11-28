namespace Linn.Stores2.Domain.LinnApps.Tests.StorageTypeServiceTests
{
    using System;
    using System.Linq.Expressions;

    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenUpdatingPcasStorageTypeButPreferenceIsNull : ContextBase
    {
        private PcasStorageType pcasStorageType, existingPcasStorageType;

        private PcasBoard pcasBoard;

        private StorageType storageType;

        private PcasStorageTypeException result;

        [SetUp]
        public void SetUp()
        {
            this.pcasBoard = new PcasBoard { BoardCode = "1", Description = "Part 1" };

            this.storageType = new StorageType { StorageTypeCode = "Storage Type No 1", };

            this.pcasStorageType = new PcasStorageType(this.pcasBoard, this.storageType, 100, 50, "1", "0");

            this.existingPcasStorageType = new PcasStorageType(this.pcasBoard, this.storageType, 50, 10, "Remark", "1");

            this.PcasBoardRepository.FindByIdAsync(Arg.Any<string>()).Returns(this.pcasBoard);

            this.StorageTypeRepository.FindByIdAsync(Arg.Any<string>()).Returns(this.storageType);

            this.PcasStorageTypeRepository.FindByAsync(
                Arg.Any<Expression<Func<PcasStorageType, bool>>>()).Returns(this.existingPcasStorageType);

            this.PcasStorageTypeRepository.FindByAsync(
                Arg.Any<Expression<Func<PcasStorageType, bool>>>()).Returns(this.existingPcasStorageType);

            this.result = Assert.ThrowsAsync<PcasStorageTypeException>(
                async () => await this.Sut.ValidatePcasStorageType(this.pcasStorageType));
        }

        [Test]
        public void ShouldThrowException()
        {
            this.result.Should().BeOfType<PcasStorageTypeException>();
        }
    }
}
