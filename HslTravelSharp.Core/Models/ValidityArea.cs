namespace HslTravelSharp.Core.Models
{
    /// <summary>
    /// Represents an area in which a ticket is valid. Can either be an HSL Zone, or
    /// a vehicle type. The <see cref="AreaType"/> property, or the <see cref="IsZone"/>
    /// and <see cref="IsVehicle"/> properties can be examined to determine whether the
    /// <see cref="ValidityZone"/> or <see cref="ValidityVehicle"/> properties have meaningful values.
    /// </summary>
    public class ValidityArea
    {        
        /// <summary>
        /// The type of validity area this object contains information for.
        /// </summary>
        public ValidityAreaType AreaType { get; private set; }

        /// <summary>
        /// The HSL fare zone that this validity area represents.
        /// </summary>
        public Zone ValidityZone { get; private set; }

        /// <summary>
        /// The vehicle type this validity area repreents.
        /// </summary>
        public Vehicle ValidityVehicle { get; private set; }

        /// <summary>
        /// Whether or not this object represents an HSL Fare Zone.
        /// </summary>
        public bool IsZone => AreaType == ValidityAreaType.Zone;

        /// <summary>
        /// Whether or not this object represents a valid vehicle.
        /// </summary>
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
