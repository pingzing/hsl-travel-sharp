namespace HslTravelSharp.Core.Models
{
    public class ValidityArea
    {        
        public ValidityAreaType AreaType { get; private set; }
        public Zone ValidityZone { get; private set; }
        public Vehicle ValidityVehicle { get; private set; }
        public bool IsZone => AreaType == ValidityAreaType.Zone;
        public bool IsVehicle => AreaType == ValidityAreaType.Vehicle;

        internal ValidityArea(Zone validityZone)
        {
            AreaType = ValidityAreaType.Zone;
            ValidityZone = validityZone;            
        }

        internal ValidityArea(Vehicle validityVehicle)
        {
            AreaType = ValidityAreaType.Vehicle;
            ValidityVehicle = validityVehicle;
        }
    }
}
