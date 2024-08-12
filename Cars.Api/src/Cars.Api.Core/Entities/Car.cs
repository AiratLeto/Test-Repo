using System;
using Cars.Api.Core.Exceptions;

namespace Cars.Api.Core.Entities
{
	/// <summary>
	/// Бибика
	/// </summary>
	public class Car : EntityBase
	{
		/// <summary>
		/// Название поля <see cref="_brand"/>
		/// </summary>
		public const string BrandField = nameof(_brand);

		/// <summary>
		/// Название поля <see cref="_bodyType"/>
		/// </summary>
		public const string BodyTypeField = nameof(_bodyType);

		private readonly string _allowedDomain = ".ru";

		private Brand? _brand;
		private BodyType? _bodyType;
		private string? _url;
		private int _seatsCount;
		private string _name = default!;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="brand">Бренд</param>
		/// <param name="name">Название модели</param>
		/// <param name="bodyType">Тип кузова</param>
		/// <param name="seatsCount">Число сидений в салоне</param>
		/// <param name="url">URL оф. дилера</param>
		public Car(
			Brand? brand,
			string name,
			BodyType? bodyType,
			int seatsCount,
			string? url)
		{
			Brand = brand;
			Name = name;
			BodyType = bodyType;
			SeatsCount = seatsCount;
			Url = url;
		}

		/// <summary>
		/// Конструктор EF
		/// </summary>
		private Car()
		{
		}

		/// <summary>
		/// Идентификатор бренда
		/// </summary>
		public Guid BrandId { get; private set; }

		/// <summary>
		/// Название модели
		/// </summary>
		public string Name
		{
			get => _name;
			set => _name = string.IsNullOrWhiteSpace(value)
				? throw new RequiredFieldNotSpecifiedException("Название модели")
				: value;
		}

		/// <summary>
		/// Идентификатор типа кузова
		/// </summary>
		public Guid BodyTypeId { get; private set; }

		/// <summary>
		/// Число сидений в салоне
		/// </summary>
		public int SeatsCount
		{
			get => _seatsCount;
			set
			{
				if (value is < 1 or > 12)
					throw new ValidationException("Число сидений должно быть от 1 до 12");

				_seatsCount = value;
			}
		}

		/// <summary>
		/// URL сайта оф. дилера
		/// </summary>
		public string? Url
		{
			get => _url;
			set
			{
				if (value?.Length > 1000)
					throw new ValidationException("URL сайта оф. дилера должен быть меньше 1000");
				if ((!value?.EndsWith(_allowedDomain)) ?? false)
					throw new ValidationException($"Сайт должен быть в домене \"{_allowedDomain}\"");

				_url = value;
			}
		}

		#region Navigation properties

		/// <summary>
		/// Бренд
		/// </summary>
		public Brand? Brand
		{
			get => _brand; set
			{
				_brand = value ?? throw new RequiredFieldNotSpecifiedException("Бренд");
				BrandId = value.Id;
			}
		}

		/// <summary>
		/// Тип кузова
		/// </summary>
		public BodyType? BodyType
		{
			get => _bodyType; set
			{
				_bodyType = value ?? throw new RequiredFieldNotSpecifiedException("Тип кузова");
				BodyTypeId = value.Id;
			}
		}

		#endregion

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="id">Идентификатор сущности</param>
		/// <param name="name">Название модели</param>
		/// <param name="seatsCount">Число сидений в салоне</param>
		/// <param name="url">URL сайта оф. дилера</param>
		/// <param name="brand">Бренд</param>
		/// <param name="bodyType">Тип кузова</param>
		/// <returns>Заполненный объект класса</returns>
		[Obsolete("Только для тестов")]
		public static Car CreateForTest(
			Guid id = default,
			string name = default!,
			int seatsCount = default,
			string? url = default,
			Brand? brand = default!,
			BodyType? bodyType = default!)
			=> new()
			{
				Id = id,
				_name = name,
				_seatsCount = seatsCount,
				_url = url,
				_brand = brand,
				BrandId = brand?.Id ?? default,
				_bodyType = bodyType,
				BodyTypeId = bodyType?.Id ?? default,
			};
	}
}
