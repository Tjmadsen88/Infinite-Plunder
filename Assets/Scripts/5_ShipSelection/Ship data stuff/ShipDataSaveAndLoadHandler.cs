using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace ShipDataSpace
{
    public static class ShipDataSaveAndLoadHandler
    {
        public static void saveData_shipData(ShipDataPacket packet, string fileName)
        {
            FileStream file = null;

            try {
                BinaryFormatter formatter = new BinaryFormatter();

                file = File.Create(Application.persistentDataPath + fileName);

                formatter.Serialize(file, packet);

            } catch (Exception e) {
                Debug.Log("There was an exception...? Saving ShipData");

            } finally {
                if (file != null)
                {
                    file.Close();
                }
            }
        }

        public static ShipDataPacket loadData_shipData(string fileName)
        {
            FileStream file = null;
            ShipDataPacket returnPacket = null;

            try {
                BinaryFormatter formatter = new BinaryFormatter();

                file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);

                returnPacket = formatter.Deserialize(file) as ShipDataPacket;
            } catch (Exception e)
            {
                Debug.Log("There was an exception...? Loading Cust");
                returnPacket = new ShipDataPacket();
            } finally {
                if (file != null)
                {
                    file.Close();
                }
            }

            //Debug.Log("Path is: " + (Application.persistentDataPath + fileName));

            return returnPacket;
        }
    }
}
