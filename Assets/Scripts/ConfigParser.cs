﻿//  =====================================================================
//  OculusExplore
//  Copyright(C)                                      
//  2017 Maksym Perepichka
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//  GNU General Public License for more details.
//            
//  You should have received a copy of the GNU General Public License 
//  along with this program.If not, see<http://www.gnu.org/licenses/>.
//  =====================================================================

using UnityEngine;
using System.Collections;
using System.IO;
using SharpConfig;

public class ConfigParser : MonoBehaviour {

    // Google Streetview API Key
    public string ParseConfig()
    {
        // Loads api key from init file. Need to specify one in order to have more than 100 reqs"
        var config = Configuration.LoadFromFile("Config\\init.ini");
        var section = config["streetview"];
        return section["API_KEY"].StringValue;
    }

    public string ParseLat()
    {
        // Loads api key from init file. Need to specify one in order to have more than 100 reqs"
        var config = Configuration.LoadFromFile("Config\\init.ini");
        var section = config["streetview"];
        return section["LAT"].StringValue;
    }

    public string ParseLng()
    {
        // Loads api key from init file. Need to specify one in order to have more than 100 reqs"
        var config = Configuration.LoadFromFile("Config\\init.ini");
        var section = config["streetview"];
        return section["LNG"].StringValue;
    }

    public string ParseVideo()
    {
        // Loads api key from init file. Need to specify one in order to have more than 100 reqs"
        var config = Configuration.LoadFromFile("Config\\init.ini");
        var section = config["video"];
        return section["VIDEO"].StringValue;
    }
    public string ParseImage()
    {
        // Loads api key from init file. Need to specify one in order to have more than 100 reqs"
        var config = Configuration.LoadFromFile("Config\\init.ini");
        var section = config["video"];
        return section["IMAGE"].StringValue;
    }
    public string ParseMono()
    {
        // Loads api key from init file. Need to specify one in order to have more than 100 reqs"
        var config = Configuration.LoadFromFile("Config\\init.ini");
        var section = config["video"];
        return section["MONO"].StringValue;
    }

}
