CREATE PROCEDURE "SBODEMOUS"."PPSOne_MultilevelBillOfMaterialStructureCost" 
(
    szvItemCode                nvarchar(50),
    szvWorkPlan                nvarchar(100),
    dQuantity                  decimal(21, 6),
    nvExplosionLevel           int,
    nvForGridUse               int,
    nvUseEconomicLotSize       int,
    szvUserLanguage            nvarchar(100),
    szvDiscountDate			   nvarchar(50)
)

AS

BEGIN
    DECLARE child                   nvarchar(4000);
    DECLARE depth                   int;
    DECLARE currentDepth            int;
    DECLARE ID                      int;
    DECLARE MAXID                   BIGINT;
    DECLARE RowCount                int;
    DECLARE CurrentRowNumber        int;
    DECLARE IChildNMPositions       int;
    
   /* teporary table for all minimal structure data*/
    CREATE LOCAL TEMPORARY TABLE #TMP
    (
        /* ID as Primary key */
        "ID"                BIGINT,
        "parent"            nvarchar(4000),
        "child"             nvarchar(4000),
        "depth"             integer,

        "NM_ArtNo"          nvarchar(50),
        "NM_WrkPlan"        nvarchar(12),
        "NM_PosNo"          integer,
        "NM_ItmArtNo"       nvarchar(50),
        "OITM_ItemName"     nvarchar(250),       
        "PriceSBO"          decimal(21,6) DEFAULT 0.0,
        "NM_Quantity"       decimal(21,6) DEFAULT 0.0,
        "Quantity"          decimal(21,6) DEFAULT 0.0,
        "MaterialCost"      decimal(21,6) DEFAULT 0.0,   
        
        "WorkCost"                 decimal(21,6) DEFAULT 0.0, 
        "WorkCostResouceListOnly"  decimal(21,6) DEFAULT 0.0, 
        "MaterialAndWorkCost" decimal(21,6) DEFAULT 0.0,
        "ChildNMPOsitions"  integer,
        "WorkPlanForWork"   nvarchar(12),
        "NM_ItmPOrig"       integer,        
        "NM_RelValue"       decimal(21,6) DEFAULT 0.0,
        "NM_RelCaSty"       integer,
        "NM_PurEnbl"        integer,
        "NM_MatIndCo"       decimal(21,6) DEFAULT 0.0,
        "NM_MaInCoUD"       integer,
        "NM_PrAssump"       decimal(21,6) DEFAULT 0.0,
        "NM_Prlino"         integer,        
        "OITM_LastPurPrc"   decimal(21,6) DEFAULT 0.0,
        "OITM_LstEvlPric"   decimal(21,6) DEFAULT 0.0,
        "OITM_U_PPSOne_I"   decimal(21,6) DEFAULT 0.0,        
        "OITM_EvalSystem"   nvarchar(50),
        "OITM_DfltWH"       nvarchar(50),
        "OITW_AvgPrice"     decimal(21,6) DEFAULT 0.0, 
        "OINM_Price"        decimal(21,6) DEFAULT 0.0, 
        "NWP_WrkPlan"       nvarchar(50),
        "NWP_FaWrkPln"      nvarchar(50),
        "NWP_IsBuyItm"      nvarchar(50),
        "NM_Code"			nvarchar(8)
        
    );

    /* fill in the first row to begin the recusion. depth 0 is allways the first row */
    INSERT INTO #TMP
    SELECT
        1            AS "ID",
        szvItemCode  AS "parent",
        'WP' ||  LEFT(szvWorkPlan || '_________________', 17)      || '_P00000'      AS "child",
        0            AS "depth",
        szvItemCode  AS "NM_ArtNo",
        szvWorkPlan  AS "NM_WrkPlan",
        '00'         AS "NM_PosNo",
        szvItemCode  AS "NM_ItmArtNo",
        ''           AS "OITM_ItemName",
        0.0          AS "PriceSBO",
        0.0          AS "NM_Quantity",
        dQuantity    AS "Quantity", 
        0.0          AS "MaterialCost",
        0.0          AS "WorkCost",
        0.0          AS "WorkCostResouceListOnly",
        0.0          AS "MaterialAndWorkCost",
        0            AS "ChildNMPOsitions",
        ''           AS "WorkPlanForWork",
        0            AS "NM_ItmPOrig",      
        0.0          AS "NM_RelValue",
        0            AS "NM_RelCaSty",
        0            AS "NM_PurEnbl",
        0.0          AS "NM_MatIndCo",
        0            AS "NM_MaInCoUD",
        0.0          AS "NM_PrAssump",
        0            AS "NM_Prlino",        
        0.0          AS "OITM_LastPurPrc",
        0.0          AS "OITM_LstEvlPric",
        0.0          AS "OITM_U_PPSOne_I",        
        ''           AS "OITM_EvalSystem",
        ''           AS "OITM_DfltWH",                          
        0.0          AS "OITW_AvgPrice",   
        0.0          AS "OINM_Price",
        szvWorkPlan  AS "NWP_WrkPlan",
        0            AS "NWP_FaWrkPln",
        0            AS "NWP_IsBuyItm",
        ''           AS "NM_Code" --2021.04.18_101-3260_PPS_1304671_Internal_DeliveryDateEnquiryReview_Step_01
                      
    FROM Dummy;
    
    

    /* set the current depth and depth to 0 */
    currentDepth := 0;
    depth := 0;
    MAXID := 1;


    /* Start Step one */   
    /* run while depth eq currentDepth */
    WHILE depth = currentDepth DO
        /* add all child lines for the current depth */
        INSERT INTO #TMP   
        SELECT 
            MAXID + ROW_NUMBER() OVER () AS "ID",
            T0."child" AS "parent",   
            T0."child" || ' ' ||  'WP' ||  LEFT("@PPSONE_NTRMATERIALS"."U_WrkPlan" || '_________________', 17)   || '_P'   || RIGHT('00000' || CAST("@PPSONE_NTRMATERIALS"."U_PosNo" AS nvarchar), 5) AS "child", 
            T0."depth" + 1,   
            "@PPSONE_NTRMATERIALS"."U_ArtNo",
            "@PPSONE_NTRMATERIALS"."U_WrkPlan",
            "@PPSONE_NTRMATERIALS"."U_PosNo",
            "@PPSONE_NTRMATERIALS"."U_ItmArtNo",
            "OITM"."ItemName",
                        
            IFNULL ( 
             	   	 (SELECT "SBODEMOUS"."PPSOne_F_CalculateNeutralMaterialPositionAverageFullPrimeCostPerUnit"("SBODEMOUS"."@PPSONE_NTRMATERIALS"."U_ArtNo", 
             	   	                                                                                                      "SBODEMOUS"."@PPSONE_NTRMATERIALS"."U_WrkPlan", 
             	   	                                                                                                      "SBODEMOUS"."@PPSONE_NTRMATERIALS"."U_ItmArtNo", 
             	   	                                                                                                      "SBODEMOUS"."@PPSONE_NTRMATERIALS"."U_PosNo",
             	   	                                                                                                      0, 
             	   	                                                                                                      szvDiscountDate)
             		 FROM DUMMY
             		
             		 )             	
	            , 0.0),
           
            "@PPSONE_NTRMATERIALS"."U_Quantity",
            0.0              AS "Quantity",
            0.0              AS "MaterialCost",     
            0.0              AS "WorkCost",  
            0.0              AS "WorkCostResouceListOnly",  
            0.0              AS "MaterialAndWorkCost",
            0                AS "ChildNMPOsitions",
            ''               AS "WorkPlanForWork",
            "@PPSONE_NTRMATERIALS"."U_ItmPOrig",            
            "@PPSONE_NTRMATERIALS"."U_RelValue",
            "@PPSONE_NTRMATERIALS"."U_RelCaSty",
            "@PPSONE_NTRMATERIALS"."U_PurEnbl",
            "@PPSONE_NTRMATERIALS"."U_MatIndCo",
            "@PPSONE_NTRMATERIALS"."U_MaInCoUD",
            "@PPSONE_NTRMATERIALS"."U_PrAssump",
            "@PPSONE_NTRMATERIALS"."U_Prlino",  
            "OITM"."LastPurPrc",
            "OITM"."LstEvlPric",
            "OITM"."U_PPSOne_I",            
            "OITM"."EvalSystem",
            "OITM"."DfltWH", 
              
            IFNULL (           
                    ( SELECT "SBODEMOUS"."OITW"."AvgPrice"
                        FROM "SBODEMOUS"."OITW"
                       WHERE "SBODEMOUS"."OITW"."ItemCode" = "SBODEMOUS"."@PPSONE_NTRMATERIALS"."U_ItmArtNo"
                         AND "SBODEMOUS"."OITW"."WhsCode" = "SBODEMOUS"."OITM"."DfltWH"
                     )
            , 0.0) ,
            
            
             IFNULL ( 
             	   	 (SELECT "SBODEMOUS"."PPSOne_F_CalculateNeutralMaterialPositionAverageFullPrimeCostPerUnit"("SBODEMOUS"."@PPSONE_NTRMATERIALS"."U_ArtNo", 
             	   	                                                                                                                  "SBODEMOUS"."@PPSONE_NTRMATERIALS"."U_WrkPlan", 
             	   	                                                                                                                  "SBODEMOUS"."@PPSONE_NTRMATERIALS"."U_ItmArtNo", 
             	   	                                                                                                                  "SBODEMOUS"."@PPSONE_NTRMATERIALS"."U_PosNo",
             	   	                                                                                                                  1, 
             	   	                                                                                                                  szvDiscountDate) 
             		 FROM DUMMY
             		
             		 )             	
	            , 0.0),

            "@PPSONE_NTRWRKPLANS"."U_WrkPlan",
            "@PPSONE_NTRWRKPLANS"."U_FaWrkPln",
            "@PPSONE_NTRWRKPLANS"."U_IsBuyItm",
            "@PPSONE_NTRMATERIALS"."Code" --2021.04.18_101-3260_PPS_1304671_Internal_DeliveryDateEnquiryReview_Step_01
                       
        FROM #TMP T0
        
       INNER JOIN  "SBODEMOUS"."@PPSONE_NTRMATERIALS" ON  T0."NM_ItmArtNo" = "@PPSONE_NTRMATERIALS"."U_ArtNo"
                                                                           AND  "SBODEMOUS"."@PPSONE_NTRMATERIALS"."U_WrkPlan" = CASE currentDepth  
                                                                                                                                      WHEN 0 
                                                                                                                                      THEN szvWorkPlan 
                                                                                                                                      ELSE (SELECT "SBODEMOUS"."PPSOne_F_WorkPlanForStructureCreation"("SBODEMOUS"."@PPSONE_NTRMATERIALS"."U_ArtNo", 
                                                                                                                                                                                                  szvWorkPlan) 
                                                                                                                                            FROM DUMMY)  
                                                                                                                                      END                                                                                   

        INNER JOIN  "SBODEMOUS"."@PPSONE_NTRWRKPLANS" ON  "SBODEMOUS"."@PPSONE_NTRWRKPLANS"."U_ArtNo" = "@PPSONE_NTRMATERIALS"."U_ArtNo"
                                                                            AND "SBODEMOUS"."@PPSONE_NTRWRKPLANS"."U_WrkPlan" = "@PPSONE_NTRMATERIALS"."U_WrkPlan"
        INNER JOIN  "SBODEMOUS"."OITM" ON  T0."NM_ItmArtNo" = "OITM"."ItemCode"
            
        WHERE T0."depth" = currentDepth
        ORDER BY "@PPSONE_NTRMATERIALS"."U_PosNo";
  
        currentDepth := currentDepth + 1;
        
        SELECT MAX("depth"), 
               MAX("ID")  
          INTO depth, 
               MAXID 
          FROM #TMP;
          
       IF currentDepth = nvExplosionLevel
           THEN
           BREAK;
       END IF;
          
    END WHILE; 


    /* set the depth variables to run from max to zero */    
    SELECT
      MAX("depth") 
      INTO depth 
    FROM #TMP;
      

  /* Calculate the quantities Top - Down*/      
   currentDepth := 1;

   WHILE currentDepth <= depth DO
    UPDATE  T0
      SET
          T0."Quantity" =  CASE (T0."NM_RelCaSty")
                            WHEN 1
                             
                              THEN  T0."Quantity" + ( SELECT "SBODEMOUS"."PPSOne_F_NeutralMaterialsAmontCalculatorFloating"(T0."NM_Quantity", T0."NM_RelValue",  (SELECT X0."Quantity"     FROM #TMP X0  WHERE X0."child" = T0."parent"))  FROM DUMMY )

                            WHEN 2
                              THEN  T0."Quantity" + ( SELECT "SBODEMOUS"."PPSOne_F_NeutralMaterialsAmontCalculatorConstant"(T0."NM_Quantity", T0."NM_RelValue")  FROM DUMMY ) 
                            WHEN 3
                              THEN  T0."Quantity" + ( SELECT "SBODEMOUS"."PPSOne_F_NeutralMaterialsAmontCalculatorRoundOfJump"(T0."NM_Quantity", T0."NM_RelValue",  (SELECT X0."Quantity"     FROM #TMP X0  WHERE X0."child" = T0."parent"))  FROM DUMMY)
                           
                           END  


      FROM #TMP T0
      WHERE T0."depth" = currentDepth;

        currentDepth := currentDepth + 1;
    END WHILE;
    
       

  /* Mark Rows beeing ResourceList */   
    SELECT
      MAX("depth") 
      INTO depth 
    FROM #TMP;
          
   currentDepth := 0;

   WHILE currentDepth <= depth DO
    UPDATE  T0
      SET
        T0."ChildNMPOsitions" = (SELECT COUNT ( X0."parent" ) FROM #TMP X0 WHERE X0."parent" = T0."child")
                                    
      FROM #TMP T0
      WHERE T0."depth" = currentDepth;

        currentDepth := currentDepth + 1;
    END WHILE;


  /* Set Correct WorkPlan for Wor Cost */
  UPDATE T0
    SET 
      T0."WorkPlanForWork" = ( SELECT MAX( X0."NWP_WrkPlan" ) FROM #TMP X0 WHERE X0."parent" = T0."child")                    
                             
  FROM  #TMP T0
  WHERE T0."ChildNMPOsitions" > 0;
  
   SELECT "ChildNMPOsitions"
     INTO IChildNMPositions
     FROM #TMP T0
      WHERE T0."depth" = 0;
 
  UPDATE T0
    SET 
    
    
     T0."WorkCostResouceListOnly" = ( SELECT "SBODEMOUS"."PPSOne_F_GetFullWorkCostForNeutralWorkplan"(T0."NM_ItmArtNo", T0."WorkPlanForWork",nvUseEconomicLotSize, T0."Quantity") FROM DUMMY) 
                           
  FROM  #TMP T0
  WHERE T0."ChildNMPOsitions" > 0;
  
  IF IChildNMPositions = 0 
  THEN
  UPDATE T0
    SET 
    
    T0."WorkCostResouceListOnly" = ( SELECT "SBODEMOUS"."PPSOne_F_GetFullWorkCostForNeutralWorkplan"(T0."NM_ArtNo", T0."NM_WrkPlan",nvUseEconomicLotSize, T0."Quantity") FROM DUMMY) 
                        
  FROM  #TMP T0;
  END IF;
 
   /* Summerize Work Cost Bottom - Up*/      
   SELECT
      MAX("depth") 
      INTO depth 
    FROM #TMP;
         
    WHILE depth >= 0 DO
     UPDATE T0
      SET
         
         T0."WorkCost" =  CASE (SELECT COUNT(*) FROM #TMP X0 WHERE X0."parent" = T0."child")
                                WHEN 0
                                  THEN
                                    T0."WorkCostResouceListOnly"   
                                ELSE
                                   ( SELECT SUM (X0."WorkCost") 
                                      FROM #TMP X0 
                                        WHERE X0."parent" = T0."child" ) + T0."WorkCostResouceListOnly"  
                              END

     FROM #TMP T0
     WHERE T0."depth" = depth
       AND T0."depth" <> 0;

        depth := depth - 1;
    END WHILE;
    
    
    
  /* Summerize Material Costs Bottom - Up*/   
    SELECT
      MAX("depth") 
      INTO depth 
    FROM #TMP;
         
    WHILE depth > 0 DO
     UPDATE T0
      SET
         
         T0."MaterialCost" =  CASE (SELECT COUNT(*) FROM #TMP X0 WHERE X0."parent" = T0."child")
                                WHEN 0
                                  THEN
                                    T0."OINM_Price"  * T0."Quantity"    
                                ELSE
                                   ( SELECT SUM (X0."MaterialCost") 
                                      FROM #TMP X0 
                                        WHERE X0."parent" = T0."child" ) * (( 100 + IFNULL((SELECT "SBODEMOUS"."OITM"."U_PPSOne_I" 
                                        									           FROM "SBODEMOUS"."OITM" 
                                        									           WHERE "SBODEMOUS"."OITM"."ItemCode" = T0."NM_ItmArtNo"),0) ) / 100)
                                    + T0."WorkCost" * (( 100 + IFNULL((SELECT "SBODEMOUS"."OITM"."U_PPSOne_I" 
                                        									           FROM "SBODEMOUS"."OITM" 
                                        									           WHERE "SBODEMOUS"."OITM"."ItemCode" = T0."NM_ItmArtNo"),0) ) / 100) - T0."WorkCost"
                                 END

     FROM #TMP T0
     WHERE T0."depth" = depth;

        depth := depth - 1;
    END WHILE;
    
    
  /* Take MaaterialOverheead out of Top Item

  

    
   /* WorkCost for most Top Resourcelist */   
   UPDATE T0
      SET
         
         T0."WorkCost" =  T0."WorkCostResouceListOnly"   
                          
     FROM #TMP T0
     WHERE T0."depth" = 0;
  
  
  
  
 
 
 
  /*Summerize Material Cost and WorkCost*/
  UPDATE T0
    SET 
    
     T0."MaterialAndWorkCost" = T0."MaterialCost" + T0."WorkCost" 
                             
  FROM  #TMP T0;
    
 
 /* Start Step three
      select all rows in the right order with position 
    
 /*Output for Debuging */
 
 /* SELECT
        LPAD('', T0."depth", 'o') AS "Level",
        "ID",
        "parent",
        "child",
        "depth",
        "NM_ArtNo"            AS "NM_ArticleNumber",
        "NM_WrkPlan"          AS "NM_Workplan",
        "NM_PosNo"            AS "NM_PositionNumber",       
        "NM_ItmArtNo"         AS "NM_ItemArticleNumber",
        "OITM_ItemName"       AS "OITM_ItemName",
        "PriceSBO"            AS "PriceSBO",
        "NM_Quantity"         AS "NM_Quantity",
        "Quantity"            AS "Quantity",
        "MaterialCost"        AS "MaterialCost",
        "WorkCost"            AS "WorkCost",
        "WorkCostResouceListOnly" AS "WorkCostResouceListOnly",
        "MaterialAndWorkCost" AS "MaterialAndWorkCost",
        "ChildNMPOsitions"    AS "ChildNMPOsitions",
        "WorkPlanForWork"     AS "WorkPlanForWork",
        "NM_ItmPOrig"         AS "NM_MaterialPositionItemArticlePriceOrigin",
        "NM_RelValue"         AS "NM_RelationValue",
        "NM_RelCaSty"         AS "NM_RelationCalcStyle",
        "NM_PurEnbl"          AS "NM_UserDefinedItemCostEnabled",
        "NM_MatIndCo"         AS "NM_MaterialIndirectCost",
        "NM_MaInCoUD"         AS "NM_UserDefinedMOHEnabled",
        "NM_PrAssump"         AS "NM_UserDefinedItemCost",
        "NM_Prlino"           AS "NM_PriceListNumber",
        "OITM_LastPurPrc"     AS "OITM_LatestFullPrimeCostPerUnit",
        "OITM_LstEvlPric"     AS "OITM_CalcFullPrimeCostPerUnit",
        "OITM_U_PPSOne_I"     AS "OITM_MaterialIndirectCost",        
        "OITM_EvalSystem"     AS "OITM_EvalSystem",
        "OITM_DfltWH"         AS "OITM_DfltWH",
        "OITW_AvgPrice"       AS "OITW_AvgPrice",
        "OINM_Price"          AS "OINM_Price", 
        "NWP_WrkPlan"         AS "NWP_Workplan",
        "NWP_FaWrkPln"        AS "NWP_FavouriteWorkplan",
        "NWP_IsBuyItm"        AS "NWP_IsMultilevelBOMBuyItem",
        "NM_Code"             AS "NM_Code" --2021.04.18_101-3260_PPS_1304671_Internal_DeliveryDateEnquiryReview_Step_01 
       
        
    FROM #TMP T0
    WHERE T0."depth" <= nvExplosionLevel
    ORDER BY "child";*/
    
    
    /*Output for View Columns */
    /* Add the sum line at the end of the table */
  
   INSERT INTO #TMP
    SELECT 
        1            AS "ID",
        ''           AS "parent",
        'ZSum'       AS "child",
        0            AS "depth",
        ''           AS "NM_ArtNo",
        ''           AS "NM_WrkPlan",
        '00'         AS "NM_PosNo",
        ''           AS "NM_ItmArtNo",
        ''           AS "OITM_ItemName",
        0.0          AS "PriceSBO",
        0.0          AS "NM_Quantity",
        dQuantity    AS "Quantity", 
        (SELECT SUM("MaterialCost") FROM #TMP T2 WHERE T2."depth" = 1), 
		(SELECT SUM("WorkCost") FROM #TMP T2 WHERE T2."depth" <= 1), 
        0.0          AS "WorkCostResouceListOnly",
        (SELECT SUM("MaterialCost") FROM #TMP T2 WHERE T2."depth" = 1) + (SELECT SUM("WorkCost") FROM #TMP T2 WHERE T2."depth" <= 1),  
        0            AS "ChildNMPOsitions",
        ''           AS "WorkPlanForWork",
        0            AS "NM_ItmPOrig",      
        0.0          AS "NM_RelValue",
        0            AS "NM_RelCaSty",
        0            AS "NM_PurEnbl",
        0.0          AS "NM_MatIndCo",
        0            AS "NM_MaInCoUD",
        0.0          AS "NM_PrAssump",
        0            AS "NM_Prlino",        
        0.0          AS "OITM_LastPurPrc",
        0.0          AS "OITM_LstEvlPric",
        0.0          AS "OITM_U_PPSOne_I",        
        ''           AS "OITM_EvalSystem",
        ''           AS "OITM_DfltWH",                          
        0.0          AS "OITW_AvgPrice",   
        0.0          AS "OINM_Price",
        ''           AS "NWP_WrkPlan",
        0            AS "NWP_FaWrkPln",
        0            AS "NWP_IsBuyItm",
        ''           AS "NM_Code" --2021.04.18_101-3260_PPS_1304671_Internal_DeliveryDateEnquiryReview_Step_01
                      
    FROM Dummy;
    
 IF nvForGridUse = 1
	THEN
		SELECT
			LPAD('', T0."depth", 'o') AS "PPSOne1",                    
			"NM_PosNo"                AS "PPSOne2", 
			"WorkPlanForWork"         AS "PPSOne3",                   
		     CASE  WHEN
					"WorkPlanForWork" <> ''
					THEN ( SELECT "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_Value"
			                 FROM "SBODEMOUS"."@PPSONE_TEXTRESOURCE"
			                WHERE "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_LangCode" =  szvUserLanguage 
				              AND "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_Section" = 'D_MC_StructurePieceListCosts_CI'
				              AND "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_NamedKey" = 'Assembly' )
					ELSE ''
					END               AS "PPSOne4",
			"NM_ItmArtNo"             AS "PPSOne5",                    

			( SELECT "SBODEMOUS"."OITM"."ItemName" FROM "SBODEMOUS"."OITM" WHERE "OITM"."ItemCode" =  T0."NM_ItmArtNo") AS "PPSOne6",    
			( SELECT "SBODEMOUS"."OITM"."FrgnName" FROM "SBODEMOUS"."OITM" WHERE "OITM"."ItemCode" =  T0."NM_ItmArtNo") AS "PPSOne7", 

			( SELECT "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_Value"
			   FROM "SBODEMOUS"."@PPSONE_TEXTRESOURCE"
			  WHERE "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_LangCode" =  szvUserLanguage 
				AND "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_Section" = 'D_MC_ProductionOrders_CI'
				AND "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_NamedKey" = 'ArticleDispositionAbrevation' ) AS "PPSOne8", 
				
			"PriceSBO"            AS "PPSOne9",
			--"NM_ItmPOrig"       AS "PPSOne10",
			CASE  WHEN
				"NM_ItmPOrig" = 2
				THEN (SELECT "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_Value" 
					       FROM "SBODEMOUS"."@PPSONE_TEXTRESOURCE" 
					      WHERE "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_LangCode" = szvUserLanguage
					         AND "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_Section" = 'D_MC_NeutralMaterials_CI' 
					         AND "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_NamedKey" = 'PPSOneCo03_2')

					WHEN "NM_ItmPOrig" = 1
					THEN (SELECT "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_Value" 
					      FROM "SBODEMOUS"."@PPSONE_TEXTRESOURCE" 
					       WHERE "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_LangCode" = szvUserLanguage
					          AND "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_Section" = 'D_MC_NeutralMaterials_CI' 
					        AND "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_NamedKey" = 'PPSOneCo03_1' )

					WHEN "NM_ItmPOrig" = 0
					THEN (SELECT "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_Value" 
					         FROM "SBODEMOUS"."@PPSONE_TEXTRESOURCE" 
					        WHERE "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_LangCode" = szvUserLanguage
					         AND "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_Section" = 'D_MC_NeutralMaterials_CI' 
					         AND "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_NamedKey" = 'PPSOneCo03_0')

				ELSE ''
					END         AS "PPSOne10",                       
			"Quantity"            AS "PPSOne11",                       
			"MaterialAndWorkCost" AS "PPSOne12", 
			 CASE  WHEN
				"WorkPlanForWork" <> ''
				THEN ( SELECT "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_Value"
						 FROM "SBODEMOUS"."@PPSONE_TEXTRESOURCE"
						WHERE "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_LangCode" =  szvUserLanguage 
						  AND "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_Section" = 'D_MC_StructurePieceListCosts_CI'
						  AND "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_NamedKey" = 'MaterialCost' )
				ELSE ''
				END               AS "PPSOne13",                      
			"MaterialCost"        AS "PPSOne14",                      
			
			"WorkCost"            AS "PPSOne15", 
			 CASE  WHEN
				"WorkPlanForWork" <> ''
					THEN ( SELECT "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_Value"
			                 FROM "SBODEMOUS"."@PPSONE_TEXTRESOURCE"
			                WHERE "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_LangCode" =  szvUserLanguage 
				              AND "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_Section" = 'D_MC_StructurePieceListCosts_CI'
				              AND "SBODEMOUS"."@PPSONE_TEXTRESOURCE"."U_NamedKey" = 'WorkCost' )
					ELSE ''
					END           AS "PPSOne16",                     
			"NM_WrkPlan"          AS "PPSOne17",                              
			"NM_ArtNo"            AS "PPSOne18",                      
			"depth"               AS "PPSOne19",                      
			"NM_WrkPlan"          AS "PPSOne20",                      

		CASE WHEN
			 (SELECT COUNT (*)
			  FROM "SBODEMOUS"."@PPSONE_NTRWRKPLANS"
			  WHERE "SBODEMOUS"."@PPSONE_NTRWRKPLANS"."U_ArtNo" =   T0."NM_ItmArtNo") > 0
		   
		 THEN (SELECT COUNT (*)
			  FROM  "SBODEMOUS"."@PPSONE_NTRWRKPLANS"
			  WHERE "SBODEMOUS"."@PPSONE_NTRWRKPLANS"."U_ArtNo" =   T0."NM_ItmArtNo") - 1
										   
		END  AS "PPSOne21",
		
		"NM_Code"                AS "PPSOne22",         -- //2021.04.18_101-3260_PPS_1304671_Internal_DeliveryDateEnquiryReview_Step_01
		"parent" 			     AS "PPSOne23", -- // 2021.05.27_101-3260_PPS_1304671_Internal_DeliveryDateEnquiryReview_Step_01
		"child"                  AS "PPSOne24"    -- //2021.05.27_101-3260_PPS_1304671_Internal_DeliveryDateEnquiryReview_Step_01      	                  
			
		FROM #TMP T0
		WHERE T0."depth" <= nvExplosionLevel
		ORDER BY "child";
	ELSE
	    SELECT
		   "MaterialCost"				AS "MaterialCost",
		   "WorkCost"					AS "WorkCost",
		   "MaterialAndWorkCost"		AS "MaterialAndWorkCost"

	     FROM #TMP T2
	     WHERE T2."child" = 'ZSum';	
	END IF;	
    
    DROP TABLE #TMP;
    
    
END;
