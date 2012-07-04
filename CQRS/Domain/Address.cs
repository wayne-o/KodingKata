namespace Sonatribe.Domain
{
	public class Address
	{
		public readonly string Street;
		public readonly string StreetNumber;
		public readonly string PostalCode;
		public readonly string City;

		public Address(string street, string number, string code, string city)
		{
			Street = street;
			StreetNumber = number;
			PostalCode = code;
			City = city;
		}
	}
}
