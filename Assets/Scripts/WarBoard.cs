using UnityEngine;

public class WarBoard : MonoBehaviour
{
    [Header("ART Assets")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Material tileMaterial;
    [SerializeField] private Material tileMaterialAlt; // Second material for alternating tiles
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material groundMaterial;
    [SerializeField] private Material borderMaterial;
    [SerializeField] private float tileSize = 1.0f;
    [SerializeField] private float yOffest = 0.2f;
    [SerializeField] private Vector3 boardCenter = Vector3.zero;

    [Header("WarBoard Settings")]
    //private const float TILE_SIZE = 1.0f;
    private const int TILE_COUNT_X = 8;
    private const int TILE_COUNT_Y = 8;
    private GameObject[,] tiles;
    private Camera currentCamera;
    private Vector2Int currentHover = -Vector2Int.one;
    private Vector3 bounds;

    private void Awake()
    {
        createAllTiles(tileSize, TILE_COUNT_X, TILE_COUNT_Y);
        createGround();
        createBorder();
        Debug.Log("NewMonoBehaviourScript Awake called");
    }

    private void Update()
    {
        if (!currentCamera)
        {
            currentCamera = Camera.main;
            return;
        }

        RaycastHit hit;
        Vector2 mousePosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        Ray ray = currentCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Tile", "Hover")))
        {
            Debug.Log($"Hit tile: {hit.transform.gameObject.name}");
            Vector2Int tileIndex = LookupTileIndex(hit.transform.gameObject);

            if (currentHover == -Vector2Int.one)
            {
                currentHover = tileIndex;
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Hover");
            }

            if (tileIndex != currentHover)
            {
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                currentHover = tileIndex;
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Hover");
            }
        }
        else
        {
            if (currentHover != -Vector2Int.one)
            {
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                currentHover = -Vector2Int.one;
            }
        }
    }

    private void createGround()
    {
        float boardWidth = TILE_COUNT_X * tileSize;
        float boardHeight = TILE_COUNT_Y * tileSize;
        
        // Create base ground only
        GameObject baseGround = GameObject.CreatePrimitive(PrimitiveType.Plane);
        baseGround.name = "BaseGround";
        baseGround.transform.parent = transform;
        
        float borderSize = 10.0f; // Extra space around the board
        float baseGroundWidth = boardWidth + borderSize * 2;
        float baseGroundHeight = boardHeight + borderSize * 2;
        
        baseGround.transform.localScale = new Vector3(baseGroundWidth / 10f, 1, baseGroundHeight / 10f);
        baseGround.transform.position = new Vector3(
            (boardWidth - tileSize) / 2, 
            -0.01f,
            (boardHeight - tileSize) / 2
        ) - bounds;
        
        // Set material
        if (groundMaterial != null)
        {
            baseGround.GetComponent<MeshRenderer>().material = groundMaterial;
        }
    }

    private void createBorder()
    {
        float boardWidth = TILE_COUNT_X * tileSize;
        float boardHeight = TILE_COUNT_Y * tileSize;
        float borderWidth = 1.0f;
        
        // Create four border pieces: top, bottom, left, right
        
        // Top border
        GameObject topBorder = GameObject.CreatePrimitive(PrimitiveType.Cube);
        topBorder.name = "TopBorder";
        topBorder.transform.parent = transform;
        topBorder.transform.localScale = new Vector3(boardWidth + 1.0f, 0.1f, borderWidth);
        topBorder.transform.position = new Vector3(
            (boardWidth - tileSize) / 2,
            yOffest + 0.05f,
            -borderWidth / 2
        ) - bounds;

        // Bottom border
        GameObject bottomBorder = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bottomBorder.name = "BottomBorder";
        bottomBorder.transform.parent = transform;
        bottomBorder.transform.localScale = new Vector3(boardWidth + 2.0f, 0.1f, borderWidth);
        bottomBorder.transform.position = new Vector3(
            boardWidth / 2,
            yOffest + 0.05f,
            boardHeight + borderWidth / 2
        ) - bounds;

        // Left border
        GameObject leftBorder = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftBorder.name = "LeftBorder";
        leftBorder.transform.parent = transform;
        leftBorder.transform.localScale = new Vector3(borderWidth, 0.1f, boardHeight + 1.0f);
        leftBorder.transform.position = new Vector3(
            -borderWidth / 2,
            yOffest + 0.05f,
            (boardHeight - tileSize) / 2
        ) - bounds;

        // Right border
        GameObject rightBorder = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightBorder.name = "RightBorder";
        rightBorder.transform.parent = transform;
        rightBorder.transform.localScale = new Vector3(borderWidth, 0.1f, boardHeight + 1.0f);
        rightBorder.transform.position = new Vector3(
            boardWidth + borderWidth / 2,
            yOffest + 0.05f,
            (boardHeight - tileSize) / 2
        ) - bounds;
        
        // Set border materials
        if (borderMaterial != null)
        {
            topBorder.GetComponent<MeshRenderer>().material = borderMaterial;
            bottomBorder.GetComponent<MeshRenderer>().material = borderMaterial;
            leftBorder.GetComponent<MeshRenderer>().material = borderMaterial;
            rightBorder.GetComponent<MeshRenderer>().material = borderMaterial;
        }
        
        // Remove colliders from borders
        DestroyImmediate(topBorder.GetComponent<BoxCollider>());
        DestroyImmediate(bottomBorder.GetComponent<BoxCollider>());
        DestroyImmediate(leftBorder.GetComponent<BoxCollider>());
        DestroyImmediate(rightBorder.GetComponent<BoxCollider>());
    }

    private void createAllTiles(float tileSize, int tileCountX, int tileCountY)
    {
        Debug.Log("Creating all tiles for the WarBoard");
        yOffest += transform.position.y;

        bounds = new Vector3( (tileCountX /2) * tileSize, 0, (tileCountY / 2) * tileSize) + boardCenter;
        tiles = new GameObject[tileCountX, tileCountY];
        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                tiles[x, y] = createTile(x, y, tileSize);
                Debug.Log($"Tile created at ({x}, {y}) with size {tileSize}");
            }
        }
    }

    private GameObject createTile(int x, int y, float tileSize)
    {
        if (x < 0 || x >= TILE_COUNT_X || y < 0 || y >= TILE_COUNT_Y)
        {
            Debug.LogError("Tile coordinates out of bounds");
            return null;
        }
        GameObject tile = new GameObject($"Tile_{x}_{y}");
        tile.transform.parent = transform;


        Mesh mesh = new Mesh();
        tile.AddComponent<MeshFilter>().mesh = mesh;
        MeshRenderer renderer = tile.AddComponent<MeshRenderer>();
        
        // Alternate materials like a chess board
        bool isEvenTile = (x + y) % 2 == 0;
        renderer.material = isEvenTile ? tileMaterial : tileMaterialAlt;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, yOffest, y * tileSize) - bounds;
        vertices[1] = new Vector3(x * tileSize, yOffest, (y + 1) * tileSize) - bounds;
        vertices[2] = new Vector3((x + 1) * tileSize, yOffest, y * tileSize) - bounds;
        vertices[3] = new Vector3((x + 1) * tileSize, yOffest, (y + 1) * tileSize) - bounds;

        int[] triangles = new int[6] { 0, 1, 2, 1, 3, 2 };

        Vector2[] uvs = new Vector2[4];
        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(0, 1);
        uvs[2] = new Vector2(1, 0);
        uvs[3] = new Vector2(1, 1);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        tile.AddComponent<BoxCollider>();

        // Create depth cube as child
        GameObject depthCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        depthCube.name = "DepthCube";
        depthCube.transform.parent = tile.transform;
        
        float depthHeight = 0.2f;
        depthCube.transform.localScale = new Vector3(tileSize, depthHeight, tileSize);
        // Position the depth cube at the center of the tile in world space
        depthCube.transform.position = new Vector3(x * tileSize + tileSize / 2, 0.2f - depthHeight / 2, y * tileSize + tileSize / 2) - bounds;
        
        // Set depth cube material (match the tile material)
        depthCube.GetComponent<MeshRenderer>().material = isEvenTile ? tileMaterial : tileMaterialAlt;
        
        // Remove collider from depth cube since parent tile handles collision
        DestroyImmediate(depthCube.GetComponent<BoxCollider>());

        tile.layer = LayerMask.NameToLayer("Tile");
        return tile;
    }

    private Vector2Int LookupTileIndex(GameObject tile)
    {
        for (int x = 0; x < TILE_COUNT_X; x++)
        {
            for (int y = 0; y < TILE_COUNT_Y; y++)
            {
                if (tiles[x, y] == tile)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        Debug.LogError("Tile not found in the WarBoard");
        return -Vector2Int.one;
    }
}
