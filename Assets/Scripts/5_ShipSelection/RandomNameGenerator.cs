using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NameGeneratorSpace
{
    public static class RandomNameGenerator
    {
        private static readonly string[] adjectives = {"Acidic", "Adorable", "Ancient", "Angry", "Aspiring", "Beautiful", "Brawny", "Budget", "Cagey", "Classy", "Confident", 
                                                        "Confused", "Cumbersome", "Cute", "Cynical", "Dangerous", "Dark", "Dashing", "Defiant", "Depressed", "Deserted", 
                                                        "Deviant", "Devilish", "Different", "Disrespectful", "Dizzy", "Dramatic", "Dusty", "Dynamic", "Easy", "Elastic", 
                                                        "Elderly", "Elegant", "Embarrassed", "Emotional", "Enchanted", "Energetic", "Fabulous", "Fantastic", "Fearful", 
                                                        "Fierce", "First", "Flaky", "Flimsy", "Flowery", "Forlorn", "Fortuitous", "Giant", "Gleaming", "Grumpy", "Guiltless", 
                                                        "Half", "Handsome", "Harmonious", "Healthy", "Heavy", "Homeless", "Hungry", "Hysterical", "Imperfect", "Industrious", 
                                                        "Irate", "Jazzy", "Jittery", "Jolly", "Joyous", "Keen", "Last", "Lazy", "Lonely", "Loving", "Lucky", "Ludicrous", 
                                                        "Luxurious", "Macabre", "Macho", "Magical", "Magnificent", "Majestic", "Mean", "Melodic", "Mighty", "Mindless", 
                                                        "Misty", "Moldy", "Naive", "Neighborly", "Nervous", "Null", "Observant", "Other", "Outstanding", "Panoramic", 
                                                        "Perfect", "Plain", "Precious", "Pretty", "Rainy", "Rebellious", "Robust", "Rusty", "Sassy", "Scrawny", "Screeching", 
                                                        "Second", "Second-Hand", "Selfish", "Shaky", "Silent", "Slippery", "Slumbering", "Smelly", "Smiling", "Sneaky", 
                                                        "Soggy", "Sophisticated", "Splendid", "Spooky", "Steady", "Strange", "Supreme", "Suspicious", "Tearful", "Third", 
                                                        "Tiny", "Tiresome", "Unfair", "Unknown", "Unusual", "Utopian", "Vague", "Vengeful", "Volatile", "Wandering", "Watery", 
                                                        "Whimsical", "Woebegone", "Young", "Zippy"};

        private static readonly string[] nouns = {"Abalone", "Albacore", "Amberjack", "Anchovy", "Angelfish", "Angelshark", "Anglerfish", "Arapaima", "Arowana", "Barnacle", 
                                                "Barracuda", "Barreleye", "Bass", "Betta", "Bichir", "Bitterling", "Blue Whale", "Bluegill", "Bonito", "Bream", "Bullfrog", 
                                                "Carp", "Catfish", "Char", "Clam", "Clownfish", "Cod", "Conch", "Coral", "Crab", "Cuttlefish", "Darter", "Dogfish", "Dolphin", 
                                                "Dorado", "Eel", "Elver", "Flounder", "Flying Fish", "Frog", "Gar", "Goby", "Goldfish", "Grayling", "Great White Shark", 
                                                "Guppy", "Haddock", "Hagfish", "Halibut", "Hammerhead Shark", "Hatchetfish", "Hermit Crab", "Herring", "Horseshoe Crab", 
                                                "Humpback Whale", "Jackfish", "Jellyfish", "Killer Whale", "Killifish", "Koi", "Krill", "Lamprey", "Lanternfish", "Loach", 
                                                "Lobster", "Mackerel", "Manatee", "Marlin", "Minnow", "Mollusk", "Mussel", "Narwhal", "Nautilus", "Needlefish", "Newt", 
                                                "Oarfish", "Octopus", "Orca", "Otter", "Oyster", "Paddlefish", "Perch", "Pike", "Piranha", "Plankton", "Pollock", "Porpoise", 
                                                "Pufferfish", "Quillfish", "Ray", "Remora", "Salamander", "Sand Dollar", "Sardine", "Sawfish", "Scallop", "Sea Bass", "Sea Urchin", 
                                                "Seahorse", "Seal", "Sealion", "Shark", "Shrimp", "Smelt", "Snail", "Snapper", "Squid", "Starfish", "Stingray", "Sturgeon", 
                                                "Sunfish", "Swordfish", "Tetra", "Tiger Shark", "Toad", "Triggerfish", "Trout", "Tuna", "Turtle", "Urchin", "Viperfish", "Walrus",
                                                "Whale", "Whale Shark", "Whelk", "Whiting", "Wrasse", "Yellowtail", "Zebrafish"};

        public static string generateRandomName()
        {
            return ("The "+ adjectives[Random.Range(0, adjectives.Length)] + " " + nouns[Random.Range(0, nouns.Length)]);
        }

        // public static string generateRandomName()
        // {
        //     string returnString = "Random Ship: ";
        //     int numOfChars = Random.Range(5, 16);

        //     for (int index=0; index<numOfChars; index++)
        //     {
        //         returnString = returnString + getRandomLetter();
        //     }

        //     return returnString;
        // }


        // private static string getRandomLetter()
        // {
        //     switch(Random.Range(0, 37))
        //     {
        //         case 0: return "A";
        //         case 1: return "B";
        //         case 2: return "C";
        //         case 3: return "D";
        //         case 4: return "E";
        //         case 5: return "F";
        //         case 6: return "G";
        //         case 7: return "H";
        //         case 8: return "I";
        //         case 9: return "J";
        //         case 10: return "K";
        //         case 11: return "L";
        //         case 12: return "M";
        //         case 13: return "N";
        //         case 14: return "O";
        //         case 15: return "P";
        //         case 16: return "Q";
        //         case 17: return "R";
        //         case 18: return "S";
        //         case 19: return "T";
        //         case 20: return "U";
        //         case 21: return "V";
        //         case 22: return "W";
        //         case 23: return "X";
        //         case 24: return "Y";
        //         case 25: return "Z";
        //         case 26: return "0";
        //         case 27: return "1";
        //         case 28: return "2";
        //         case 29: return "3";
        //         case 30: return "4";
        //         case 31: return "5";
        //         case 32: return "6";
        //         case 33: return "7";
        //         case 34: return "8";
        //         case 35: return "9";
        //         default: return "_";
        //     }
        // }
    }
}
