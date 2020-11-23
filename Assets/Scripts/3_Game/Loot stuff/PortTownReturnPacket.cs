using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortTownSpace
{
    public class PortTownReturnPacket
    {
        private PortTownIndividualPacket[] portTownArray;
        private byte numOfPortTowns_ideal;
        private byte numOfPortTowns_actual = 0;

        // private Color32[] portTownRoofColors;

        public PortTownReturnPacket(byte numOfPortTowns_ideal)
        {
            this.numOfPortTowns_ideal = numOfPortTowns_ideal;
            portTownArray = new PortTownIndividualPacket[numOfPortTowns_ideal];
        }


        public void addNewPortTownData(PortTownIndividualPacket portTownData)
        {
            portTownArray[numOfPortTowns_actual] = portTownData;
            numOfPortTowns_actual++;
        }


        public bool checkIfCoordinateIsTooCloseToAnotherPortTown(byte xCoord, byte yCoord)
        {
            for (byte index=0; index<numOfPortTowns_actual; index++)
            {
                if (Mathf.Abs(xCoord - portTownArray[index].getPortTownCoords_simple()[0]) <= 1 &&
                    Mathf.Abs(yCoord - portTownArray[index].getPortTownCoords_simple()[1]) <= 1)
                    return true;
            }

            return false;
        }


        public PortTownIndividualPacket[] getPortTownArray()
        {
            return portTownArray;
        }

        public byte getNumofPortTowns_actual()
        {
            return numOfPortTowns_actual;
        }

        public Vector3 getPortTownLocation_asVector3(byte portTownIndex)
        {
            return new Vector3(portTownArray[portTownIndex].getPortTownCoords_upscaled()[0], 0f, portTownArray[portTownIndex].getPortTownCoords_upscaled()[1]);
        }

        public Vector3 getPortTownRotation_asVector3(byte portTownIndex)
        {
            return new Vector3(0f, portTownArray[portTownIndex].getPortTownYRotation(), 0f);
        }
    }
}
