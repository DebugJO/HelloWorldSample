CREATE TABLE dbo.TestTable (
  MenuID int NOT NULL,
  MenuName varchar(100) COLLATE Korean_Wansung_CI_AS NOT NULL,
  ParentID int NULL,
  CONSTRAINT PK_TestTable_MenuID PRIMARY KEY CLUSTERED (MenuID)
    WITH (
      PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF,
      ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_TestTable_MenuName ON dbo.TestTable
  (MenuName)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_TestTable_ParentID ON dbo.TestTable
  (ParentID)
WITH (
  PAD_INDEX = OFF,
  DROP_EXISTING = OFF,
  STATISTICS_NORECOMPUTE = OFF,
  SORT_IN_TEMPDB = OFF,
  ONLINE = OFF,
  ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO
  
INSERT INTO TestTable (MenuID, MenuName, ParentID) VALUES (1, 'Root', NULL);
INSERT INTO TestTable (MenuID, MenuName, ParentID) VALUES (2, 'Child 1', 1);
INSERT INTO TestTable (MenuID, MenuName, ParentID) VALUES (3, 'Child 2', 1);
INSERT INTO TestTable (MenuID, MenuName, ParentID) VALUES (4, 'Grandchild 1', 2);
INSERT INTO TestTable (MenuID, MenuName, ParentID) VALUES (5, 'Grandchild 2', 2);
INSERT INTO TestTable (MenuID, MenuName, ParentID) VALUES (6, 'Grandchild 3', 3);
GO

select * from TestTable
GO

/* 재귀조회 */
WITH MenuHierarchy AS (
    -- 앵커 멤버: 최상위 노드를 선택합니다.
    SELECT
        MenuID,
        MenuName,
        ParentID,
        CAST(MenuName AS NVARCHAR(MAX)) AS HierarchyPath,
        0 AS Level
    FROM TestTable
    WHERE ParentID IS NULL

    UNION ALL

    -- 재귀 멤버: 부모-자식 관계를 통해 하위 노드를 선택합니다.
    SELECT
        m.MenuID,
        m.MenuName,
        m.ParentID,
        CAST(h.HierarchyPath + ' > ' + m.MenuName AS NVARCHAR(MAX)) AS HierarchyPath,
        h.Level + 1
    FROM TestTable m
    INNER JOIN MenuHierarchy h ON m.ParentID = h.MenuID
)

-- 최종 결과를 선택합니다.
SELECT
    MenuID,
    MenuName,
    ParentID,
    HierarchyPath,
    Level
FROM MenuHierarchy
ORDER BY HierarchyPath;


/* 각 메뉴 항목의 자식 조회 */
WITH MenuChildren AS (
    -- 앵커 멤버: 최상위 노드를 선택합니다.
    SELECT
        MenuID,
        MenuName,
        ParentID,
        CAST(MenuName AS NVARCHAR(MAX)) AS HierarchyPath,
        0 AS Depth
    FROM TestTable
    WHERE ParentID IS NULL

    UNION ALL

    -- 재귀 멤버: 부모-자식 관계를 통해 하위 노드를 선택합니다.
    SELECT
        m.MenuID,
        m.MenuName,
        m.ParentID,
        CAST(h.HierarchyPath + ' > ' + m.MenuName AS NVARCHAR(MAX)) AS HierarchyPath,
        h.Depth + 1
    FROM TestTable m
    INNER JOIN MenuChildren h ON m.ParentID = h.MenuID
)

-- 최종 결과를 선택합니다.
SELECT
    ParentID AS MenuID,
    STRING_AGG(MenuName, ', ') AS Children
FROM MenuChildren
GROUP BY ParentID
ORDER BY ParentID;

/* 각 자식 메뉴 항목의 부모 조회 */
SELECT
    m1.MenuID AS ChildID,
    m1.MenuName AS ChildName,
    m2.MenuID AS ParentID,
    m2.MenuName AS ParentName
FROM TestTable m1
LEFT JOIN TestTable m2 ON m1.ParentID = m2.MenuID
ORDER BY m1.MenuID;
