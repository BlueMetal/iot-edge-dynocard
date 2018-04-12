    Public Function ToUnix(dt)
        ToUnix = DateDiff("s", "1/1/1970", dt)
    End Function

    Public Function ShiftRight(ByVal Value, ByVal Shift)
        Dim i 
        ShiftRight = Value
        If Shift > 0 Then
            ShiftRight = Int(ShiftRight / (2 ^ Shift))
        End If
    End Function

    Sub SetCurrentDateTime(CardType, startRegister, registerType)
        Dim timeStamp
        Dim dateValue
        Dim rightValue
        Dim leftValue
        Dim leftRegister
        Dim rightRegister 

        'If (CardType = "Surface") Then
            leftRegister = startRegister
            rightRegister = startRegister + 1
        'Else If (CardType = "Pump") Then
        '    leftRegister = startRegister
        '    rightRegister = startRegister + 1
        'End If

        timeStamp = Now()
        dateValue = ToUnix(timeStamp)
        leftValue = ShiftRight(dateValue, 16)
        rightValue = dateValue And &HFFFF&

        SetRegisterValue registerType, leftRegister, leftValue
        SetRegisterValue registerType, rightRegister, rightValue
        
    End Sub


        Dim startRegister
        startRegister = 2668
        
        Dim registerType
        registerType = 2

		currentDate = DateDiff("s", "1/1/1970", Now())
        
        If (currentDate Mod 5) = 0 Then
        
	        'Set surface card values
	        Call SetCurrentDateTime("Surface", startRegister, registerType)
	
	        'Set Number of points in the array
	        SetRegisterValue registerType, startRegister + 2, 400
	
	        'Set the scaled min and max load
	        SetRegisterValue registerType, startRegister + 3, 19500
	        SetRegisterValue registerType, startRegister + 4, 7500
	
	        'Set stroke length and period
	        SetRegisterValue registerType, startRegister + 5, 1200
	        SetRegisterValue registerType, startRegister + 6, 150
			
	        'Set the load and position array - static array that will shift over time
	        loadArray = Array(11744, 11259, 10801, 10667, 10804, 10764, 10892, 11126, 11401, 11518, 11576, 11713, 11910, 11992, 12031, 12082, 12130, 12146, 12132, 12223, 12279, 12232, 12169, 12076, 11790, 11379, 11088, 10719, 10368, 10016, 9631, 9424, 9237, 9072, 8931, 8840, 8722, 8590, 8457, 8433, 8411, 8463, 8657, 8812, 9127, 9511, 9867, 10305, 10656, 10878, 10928, 10896, 10804, 10718, 10705, 10647, 10552, 10469, 10453, 10121, 9890, 9672, 9319, 8831, 8377, 7970, 7613, 7382, 7376, 7592, 7879, 8156, 8443, 8734, 8907, 9155, 9395, 9588, 9852, 10211, 10556, 10826, 11122, 11468, 11848, 12227, 12387, 12538, 12775, 13073, 13177, 13320, 13423, 13492, 13581, 13709, 13882, 14077, 14262, 14271, 14348, 14501, 14693, 14674, 14902, 15352, 15854, 15996, 15819, 15682, 15544, 15319, 14984, 14658, 14466, 14406, 14090, 14042, 14031, 13973, 13817, 13586, 13556, 13671, 13840, 14052, 14286, 14482, 14666, 15235, 15564, 15774, 15937, 16170, 16376, 16458, 16461, 16643, 16740, 16857, 16943, 16721, 16541, 16416, 16309, 16066, 15702, 15401, 15173, 14858, 14570, 14513, 14688, 14928, 15099, 15267, 15435, 15544, 15788, 16127, 16511, 16694, 17070, 17531, 18030, 18565, 18999, 19384, 19802, 19594, 19231, 18816, 18450, 18188, 17918, 17570, 17306, 17060, 16792, 16453, 16079, 15729, 15406, 15109, 14637, 14136, 13690, 13440, 13211, 13076, 13022, 13010, 12866, 12821, 12812, 12707, 12525, 12356, 12243, 12229)
	        positionArray = Array(1, 16, 33, 50, 63, 114, 165, 216, 268, 346, 430, 515, 600, 708, 825, 942, 1058, 1193, 1338, 1483, 1628, 1786, 1953, 2120, 2288, 2464, 2646, 2828, 3009, 3195, 3386, 3576, 3766, 3958, 4151, 4344, 4537, 4730, 4921, 5113, 5304, 5495, 5682, 5869, 6055, 6244, 6424, 6604, 6784, 6965, 7138, 7308, 7477, 7646, 7810, 7968, 8126, 8283, 8437, 8582, 8727, 8872, 9015, 9147, 9279, 9410, 9541, 9662, 9780, 9900, 10019, 10131, 10238, 10345, 10453, 10555, 10650, 10744, 10840, 10932, 11013, 11095, 11178, 11259, 11327, 11395, 11464, 11533, 11586, 11640, 11694, 11747, 11787, 11825, 11863, 11904, 11925, 11945, 11966, 11986, 11993, 11995, 11996, 11998, 11986, 11968, 11949, 11930, 11899, 11858, 11818, 11778, 11725, 11662, 11599, 11536, 11462, 11376, 11290, 11204, 11108, 11000, 10892, 10783, 10667, 10537, 10409, 10280, 10147, 10000, 9853, 9707, 9558, 9396, 9235, 9073, 8912, 8738, 8565, 8392, 8219, 8038, 7856, 7674, 7491, 7304, 7115, 6925, 6735, 6544, 6351, 6159, 5967, 5775, 5584, 5393, 5202, 5013, 4827, 4641, 4456, 4271, 4094, 3917, 3741, 3566, 3396, 3231, 3065, 2899, 2736, 2582, 2427, 2271, 2116, 1975, 1835, 1694, 1548, 1426, 1303, 1180, 1056, 946, 844, 743, 641, 550, 473, 397, 321, 252, 204, 157, 109, 65, 49, 32, 16, 8)
	
	        Dim load 
	        Dim position
	        Dim loadRegister 
	        Dim positionRegister
	        Dim positionValue
	        Dim arrayStartRegister
	        Dim currentDate
                
	        arrayStartRegister = startRegister + 7
	
	        Call SetCurrentDateTime("Surface", startRegister, registerType)
	
	        For index = 0 To UBound(loadArray)
	            
	            loadRegister = arrayStartRegister + (index * 2)
		        positionRegister = loadRegister + 1
	            
	            load = loadArray(index)
	            position = positionArray(index)
	            
	            positionValue = GetRegisterValue(registerType, positionRegister)
	            
	            If positionValue > 0 Then
	            	position = positionValue + 1
	            End If
	            
	            'WScript.Echo "Register: " & index2 & ", Value: "& position
	            
	            SetRegisterValue registerType, loadRegister, load
	            SetRegisterValue registerType, positionRegister, position
	        Next
	        
	        startRegister = 4703
	        arrayStartRegister = startRegister + 9
	        
	        'Set surface card values
	        Call SetCurrentDateTime("Pump", startRegister, registerType)
	
	        'Set the scaled min and max load
	        SetRegisterValue registerType, startRegister + 2, 18500
	        SetRegisterValue registerType, startRegister + 3, 6500
	
			'Set Number of points in the array
	        SetRegisterValue registerType, startRegister + 4, 200
	
	        
	        'Set net and gross stroke length
	        SetRegisterValue registerType, startRegister + 5, 1200
	        SetRegisterValue registerType, startRegister + 6, 150
	        
	         'Set pump fillage and fluid load
	        SetRegisterValue registerType, startRegister + 7, 77
	        SetRegisterValue registerType, startRegister + 8, 9500
	        
	        Dim pumpIndex
	        pumpIndex = 0
	        
	        For index = 0 To 100
	         	loadRegister = arrayStartRegister + (index * 2)
		        positionRegister = loadRegister + 1
	        	
	        	load = loadArray(index)
	            position = positionArray(index) 
	        	
	        	positionValue = GetRegisterValue(registerType, positionRegister)
	            
	            If positionValue > 0 Then
	            	position = positionValue + 1
	            End If
	            
	            'WScript.Echo "Register: " & index2 & ", Value: "& position
	            
	            SetRegisterValue registerType, loadRegister, load
	            SetRegisterValue registerType, positionRegister, position
	        Next
	        
		End If