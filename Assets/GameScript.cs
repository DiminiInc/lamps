using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public GameObject diode;
    public GameObject lamp;
    public GameObject source;
    public GameObject switchCorner;
    public GameObject switchLine;
    public GameObject switchBlank;
    public GameObject wireCorner;
    public GameObject wire;
    public GameObject target;
    public Button restartButton;

    private Field[,] fieldArray;

    private Vector2 lampPosition;
    private Vector2 position;
    private int lampPositionX;
    private int lampPositionY;
    private int targetX;
    private int targetY;
    private GameObject targ;

    private int checkX;
    private int checkY;
    private int checkNextX;
    private int checkNextY;
    private int lampCount;
    private int lampCountMax;
    private bool cycle;

    private int remainedActions;
    private int score;
    private Button btn;

    public Texture2D textureGray;

    private Vector2 sourcePosition;

    struct Field
    {
        public int rotation;
        public int piece;
        public GameObject tile;
        public int firstX;
        public int firstY;
        public int secondX;
        public int secondY;
    };
    [SerializeField] public Texture gray;
    void OnGUI()
    {
        GUI.contentColor = Color.gray;

        GUI.Label(new Rect(10, 10, 150, 20), ("████████████████████████"));
        GUI.Label(new Rect(10, 20, 150, 20), ("████████████████████████"));
        GUI.Label(new Rect(10, 30, 150, 20), ("████████████████████████"));
        GUI.contentColor = Color.black;
        if (cycle)
        {
            GUI.Label(new Rect(10, 10, 100, 20), ("Lamps in line: " + lampCount));
        } else
        {
            GUI.Label(new Rect(10, 10, 100, 20), ("Lamps in line: 0 (" + lampCount+")"));
        }
        GUI.Label(new Rect(10, 20, 200, 20), ("Remained actions: "+remainedActions));
        GUI.Label(new Rect(10, 30, 100, 20), ("Score: "+score));

        //GUI.Label(new Rect(10, 40, 100, 100), gray);
        //GUI.Label(new Rect(10, 10, 100, 100), "Label");
        //GUI.Label(new Rect(10, 40, 100, 100), textureGray);

    }

    void checkLine()
    {
        lampCount = 0;
        checkX = 50;
        checkY = 50;
        checkNextX = 1;
        checkNextY = 0;
        cycle = false;
        while (fieldArray[checkX+checkNextX,checkY+checkNextY].firstX==-checkNextX && fieldArray[checkX + checkNextX, checkY + checkNextY].firstY == -checkNextY || 
            fieldArray[checkX + checkNextX, checkY + checkNextY].secondX == -checkNextX && fieldArray[checkX + checkNextX, checkY + checkNextY].secondY == -checkNextY)
        {
            //Debug.Log("x" + checkX + "y" + checkY+"x" + checkNextX + "y" + checkNextY+ 
            //    "x" + (checkX + checkNextX) + "y" + (checkY + checkNextY) + 
            //    "x" + fieldArray[checkX + checkNextX, checkY + checkNextY].firstX + "y" + fieldArray[checkX + checkNextX, checkY + checkNextY].firstY + 
            //    "x" + fieldArray[checkX + checkNextX, checkY + checkNextY].secondX + "y" + fieldArray[checkX + checkNextX, checkY + checkNextY].secondY);
            checkX = checkX + checkNextX;
            checkY = checkY + checkNextY;
            if (fieldArray[checkX, checkY].firstX == -checkNextX && fieldArray[checkX, checkY].firstY == -checkNextY)
            {
                checkNextX = fieldArray[checkX, checkY].secondX;
                checkNextY = fieldArray[checkX, checkY].secondY;
            }
            else
            {
                checkNextX = fieldArray[checkX, checkY].firstX;
                checkNextY = fieldArray[checkX, checkY].firstY;
            }
            if (fieldArray[checkX, checkY].piece==0 || fieldArray[checkX, checkY].piece == 1)
            {
                lampCount++;
            }
            if (checkX==50 && checkY == 50)
            {
                cycle = true;
                break;
            }
        }
        if ((lampCount > lampCountMax) && cycle)
        {
            remainedActions += 10 * lampCountMax;
            lampCountMax = lampCount;
            score += 100;
        }
        //Debug.Log("F x" + checkX + "y" + checkY + "x" + checkNextX + "y" + checkNextY +
        //       "x" + (checkX + checkNextX) + "y" + (checkY + checkNextY) +
        //       "x" + fieldArray[checkX + checkNextX, checkY + checkNextY].firstX + "y" + fieldArray[checkX + checkNextX, checkY + checkNextY].firstY +
        //       "x" + fieldArray[checkX + checkNextX, checkY + checkNextY].secondX + "y" + fieldArray[checkX + checkNextX, checkY + checkNextY].secondY);
        //Debug.Log("lamps"+lampCount+"cycle"+cycle);
    }

    public void RestartAction()
    {
        Start();
    }

    // Start is called before the first frame update
    void Start()
    {

        fieldArray = new Field[100, 100];
        remainedActions = 100;
        score = 0;
        lampCountMax=0;
        btn = restartButton.GetComponent<Button>();
        Debug.Log(btn);
        btn.onClick.AddListener(RestartAction);
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                fieldArray[i, j].piece = (int)Random.Range(0, 10);
                fieldArray[i, j].rotation = (int)Random.Range(0, 4) * 90;
                position = new Vector2(i + 0.5f, j + 0.5f);
                fieldArray[50, 50].piece = 10;
                fieldArray[50, 50].rotation = 0;
                fieldArray[49, 50].piece = 6;
                fieldArray[49, 50].rotation = 90;
                fieldArray[49, 49].piece = 6;
                fieldArray[49, 49].rotation = 180;
                fieldArray[51, 50].piece = 6;
                fieldArray[51, 50].rotation = 0;
                fieldArray[51, 49].piece = 6;
                fieldArray[51, 49].rotation = 270;
                fieldArray[50, 49].piece = 2;
                fieldArray[50, 49].rotation = 0;
                switch (fieldArray[i, j].piece)
                {
                    case 0:
                    case 1:
                        fieldArray[i,j].tile = Instantiate(lamp, position, Quaternion.Euler(new Vector3(0, 0, fieldArray[i, j].rotation)));
                        if (fieldArray[i,j].rotation == 0 || fieldArray[i,j].rotation == 180)
                        {
                            fieldArray[i, j].firstX = 1;
                            fieldArray[i, j].secondX = -1;
                            fieldArray[i, j].firstY = 0;
                            fieldArray[i, j].secondY = 0;
                        }
                        else
                        {
                            fieldArray[i, j].firstX = 0;
                            fieldArray[i, j].secondX = 0;
                            fieldArray[i, j].firstY = 1;
                            fieldArray[i, j].secondY = -1;
                        }
                        break;
                    //case 1:
                    //    fieldArray[i, j].tile = Instantiate(switchBlank, position, Quaternion.Euler(new Vector3(0, 0, fieldArray[i, j].rotation)));

                    //    break;
                    case 2:
                    case 3:
                        fieldArray[i, j].tile = Instantiate(wire, position, Quaternion.Euler(new Vector3(0, 0, fieldArray[i, j].rotation)));
                        if (fieldArray[i, j].rotation == 0 || fieldArray[i, j].rotation == 180)
                        {
                            fieldArray[i, j].firstX = 1;
                            fieldArray[i, j].secondX = -1;
                            fieldArray[i, j].firstY = 0;
                            fieldArray[i, j].secondY = 0;
                        }
                        else
                        {
                            fieldArray[i, j].firstX = 0;
                            fieldArray[i, j].secondX = 0;
                            fieldArray[i, j].firstY = 1;
                            fieldArray[i, j].secondY = -1;
                        }
                        break;

                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        fieldArray[i, j].tile = Instantiate(wireCorner, position, Quaternion.Euler(new Vector3(0, 0, fieldArray[i, j].rotation)));
                        switch (fieldArray[i, j].rotation)
                        {
                            case 0:
                                fieldArray[i, j].firstX = -1;
                                fieldArray[i, j].secondX = 0;
                                fieldArray[i, j].firstY = 0;
                                fieldArray[i, j].secondY = -1;
                                break;
                            case 90:
                                fieldArray[i, j].firstX = 1;
                                fieldArray[i, j].secondX = 0;
                                fieldArray[i, j].firstY = 0;
                                fieldArray[i, j].secondY = -1;
                                break;
                            case 180:
                                fieldArray[i, j].firstX = 1;
                                fieldArray[i, j].secondX = 0;
                                fieldArray[i, j].firstY = 0;
                                fieldArray[i, j].secondY = 1;
                                break;
                            case 270:
                                fieldArray[i, j].firstX = -1;
                                fieldArray[i, j].secondX = 0;
                                fieldArray[i, j].firstY = 0;
                                fieldArray[i, j].secondY = 1;
                                break;
                        }
                        break;
                    case 10:
                        fieldArray[i, j].tile = Instantiate(source, position, Quaternion.Euler(new Vector3(0, 0, fieldArray[i, j].rotation)));
                        if (fieldArray[i, j].rotation == 0 || fieldArray[i, j].rotation == 180)
                        {
                            fieldArray[i, j].firstX = 1;
                            fieldArray[i, j].secondX = -1;
                            fieldArray[i, j].firstY = 0;
                            fieldArray[i, j].secondY = 0;
                        }
                        else
                        {
                            fieldArray[i, j].firstX = 0;
                            fieldArray[i, j].secondX = 0;
                            fieldArray[i, j].firstY = 1;
                            fieldArray[i, j].secondY = -1;
                        }
                        break;
                }
            }
        }
        //for (int i = 0; i < 100; i++)
        //{
        //    for (int j = 0; j < 100; j++)
        //    {
        //        switch (fieldArray[i, j].piece)
        //        {
        //            case 1:
        //                fieldArray[i, j].tile = Instantiate(switchBlank, position, Quaternion.Euler(new Vector3(0, 0, fieldArray[i, j].rotation)));

        //                break;
                
        //        }
        //    }
        //}
        targetX = 50;
        targetY = 50;
        position = new Vector2(targetX + 0.5f, targetY + 0.5f);
        targ = Instantiate(target, position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (remainedActions > 0)
        {
            Vector3 cursorInWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            Vector2Int targetTile = (Vector2Int)Vector3Int.FloorToInt(cursorInWorld);
            position = new Vector2(targetTile.x + 0.5f, targetTile.y + 0.5f);
            //targ.transform.position = position;
            //GameObject.Find("Main Camera").transform.position = new Vector3(targetTile.x, targetTile.y, -10);
            if (Input.GetMouseButtonDown(0))
            {
                if (fieldArray[targetTile.x, targetTile.y].piece != 10)
                {
                    fieldArray[targetTile.x, targetTile.y].rotation = fieldArray[targetTile.x, targetTile.y].rotation + 90;
                }
                if (fieldArray[targetTile.x, targetTile.y].rotation == 360)
                {
                    fieldArray[targetTile.x, targetTile.y].rotation = 0;
                }
                fieldArray[targetTile.x, targetTile.y].tile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, fieldArray[targetTile.x, targetTile.y].rotation));
                //Debug.Log(targetTile);
                switch (fieldArray[targetTile.x, targetTile.y].piece)
                {
                    case 0:
                    case 1:
                        if (fieldArray[targetTile.x, targetTile.y].rotation == 0 || fieldArray[targetTile.x, targetTile.y].rotation == 180)
                        {
                            fieldArray[targetTile.x, targetTile.y].firstX = 1;
                            fieldArray[targetTile.x, targetTile.y].secondX = -1;
                            fieldArray[targetTile.x, targetTile.y].firstY = 0;
                            fieldArray[targetTile.x, targetTile.y].secondY = 0;
                        }
                        else
                        {
                            fieldArray[targetTile.x, targetTile.y].firstX = 0;
                            fieldArray[targetTile.x, targetTile.y].secondX = 0;
                            fieldArray[targetTile.x, targetTile.y].firstY = 1;
                            fieldArray[targetTile.x, targetTile.y].secondY = -1;
                        }
                        break;
                    //case 1:

                    //    break;
                    case 2:
                    case 3:
                        if (fieldArray[targetTile.x, targetTile.y].rotation == 0 || fieldArray[targetTile.x, targetTile.y].rotation == 180)
                        {
                            fieldArray[targetTile.x, targetTile.y].firstX = 1;
                            fieldArray[targetTile.x, targetTile.y].secondX = -1;
                            fieldArray[targetTile.x, targetTile.y].firstY = 0;
                            fieldArray[targetTile.x, targetTile.y].secondY = 0;
                        }
                        else
                        {
                            fieldArray[targetTile.x, targetTile.y].firstX = 0;
                            fieldArray[targetTile.x, targetTile.y].secondX = 0;
                            fieldArray[targetTile.x, targetTile.y].firstY = 1;
                            fieldArray[targetTile.x, targetTile.y].secondY = -1;
                        }
                        break;
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        switch (fieldArray[targetTile.x, targetTile.y].rotation)
                        {
                            case 0:
                                fieldArray[targetTile.x, targetTile.y].firstX = -1;
                                fieldArray[targetTile.x, targetTile.y].secondX = 0;
                                fieldArray[targetTile.x, targetTile.y].firstY = 0;
                                fieldArray[targetTile.x, targetTile.y].secondY = -1;
                                break;
                            case 90:
                                fieldArray[targetTile.x, targetTile.y].firstX = 1;
                                fieldArray[targetTile.x, targetTile.y].secondX = 0;
                                fieldArray[targetTile.x, targetTile.y].firstY = 0;
                                fieldArray[targetTile.x, targetTile.y].secondY = -1;
                                break;
                            case 180:
                                fieldArray[targetTile.x, targetTile.y].firstX = 1;
                                fieldArray[targetTile.x, targetTile.y].secondX = 0;
                                fieldArray[targetTile.x, targetTile.y].firstY = 0;
                                fieldArray[targetTile.x, targetTile.y].secondY = 1;
                                break;
                            case 270:
                                fieldArray[targetTile.x, targetTile.y].firstX = -1;
                                fieldArray[targetTile.x, targetTile.y].secondX = 0;
                                fieldArray[targetTile.x, targetTile.y].firstY = 0;
                                fieldArray[targetTile.x, targetTile.y].secondY = 1;
                                break;
                        }
                        break;
                    case 10:
                        remainedActions++;
                        score--;
                        //if (fieldArray[targetTile.x, targetTile.y].rotation == 0 || fieldArray[targetTile.x, targetTile.y].rotation == 180)
                        //{
                        //    fieldArray[targetTile.x, targetTile.y].firstX = 1;
                        //    fieldArray[targetTile.x, targetTile.y].secondX = -1;
                        //    fieldArray[targetTile.x, targetTile.y].firstY = 0;
                        //    fieldArray[targetTile.x, targetTile.y].secondY = 0;
                        //}
                        //else
                        //{
                        //    fieldArray[targetTile.x, targetTile.y].firstX = 0;
                        //    fieldArray[targetTile.x, targetTile.y].secondX = 0;
                        //    fieldArray[targetTile.x, targetTile.y].firstY = 1;
                        //    fieldArray[targetTile.x, targetTile.y].secondY = -1;
                        //}
                        break;
                }
                remainedActions--;
                score++;
            }
            if (Input.GetKeyDown("left"))
            {
                //replace rail
                targetX = targetX - 1;
                position = new Vector2(targetX + 0.5f, targetY + 0.5f);
                targ.transform.position = position;
            }
            if (Input.GetKeyDown("up"))
            {
                //replace rail
                targetY = targetY + 1;
                position = new Vector2(targetX + 0.5f, targetY + 0.5f);
                targ.transform.position = position;
            }
            if (Input.GetKeyDown("right"))
            {
                //replace rail
                targetX = targetX + 1;
                position = new Vector2(targetX + 0.5f, targetY + 0.5f);
                targ.transform.position = position;
            }
            if (Input.GetKeyDown("down"))
            {
                //replace rail
                targetY = targetY - 1;
                position = new Vector2(targetX + 0.5f, targetY + 0.5f);
                targ.transform.position = position;
            }
            GameObject.Find("Main Camera").transform.position = new Vector3(targetX, targetY, -10);
            if (Input.GetKeyDown("space"))
            {
                if (fieldArray[targetX, targetY].piece != 10)
                {
                    fieldArray[targetX, targetY].rotation = fieldArray[targetX, targetY].rotation + 90;
                }
                if (fieldArray[targetX, targetY].rotation == 360)
                {
                    fieldArray[targetX, targetY].rotation = 0;
                }
                fieldArray[targetX, targetY].tile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, fieldArray[targetX, targetY].rotation));
                switch (fieldArray[targetX, targetY].piece)
                {
                    case 0:
                    case 1:
                        if (fieldArray[targetX, targetY].rotation == 0 || fieldArray[targetX, targetY].rotation == 180)
                        {
                            fieldArray[targetX, targetY].firstX = 1;
                            fieldArray[targetX, targetY].secondX = -1;
                            fieldArray[targetX, targetY].firstY = 0;
                            fieldArray[targetX, targetY].secondY = 0;
                        }
                        else
                        {
                            fieldArray[targetX, targetY].firstX = 0;
                            fieldArray[targetX, targetY].secondX = 0;
                            fieldArray[targetX, targetY].firstY = 1;
                            fieldArray[targetX, targetY].secondY = -1;
                        }
                        break;
                    //case 1:

                    //    break;
                    case 2:
                    case 3:
                        if (fieldArray[targetX, targetY].rotation == 0 || fieldArray[targetX, targetY].rotation == 180)
                        {
                            fieldArray[targetX, targetY].firstX = 1;
                            fieldArray[targetX, targetY].secondX = -1;
                            fieldArray[targetX, targetY].firstY = 0;
                            fieldArray[targetX, targetY].secondY = 0;
                        }
                        else
                        {
                            fieldArray[targetX, targetY].firstX = 0;
                            fieldArray[targetX, targetY].secondX = 0;
                            fieldArray[targetX, targetY].firstY = 1;
                            fieldArray[targetX, targetY].secondY = -1;
                        }
                        break;
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        switch (fieldArray[targetX, targetY].rotation)
                        {
                            case 0:
                                fieldArray[targetX, targetY].firstX = -1;
                                fieldArray[targetX, targetY].secondX = 0;
                                fieldArray[targetX, targetY].firstY = 0;
                                fieldArray[targetX, targetY].secondY = -1;
                                break;
                            case 90:
                                fieldArray[targetX, targetY].firstX = 1;
                                fieldArray[targetX, targetY].secondX = 0;
                                fieldArray[targetX, targetY].firstY = 0;
                                fieldArray[targetX, targetY].secondY = -1;
                                break;
                            case 180:
                                fieldArray[targetX, targetY].firstX = 1;
                                fieldArray[targetX, targetY].secondX = 0;
                                fieldArray[targetX, targetY].firstY = 0;
                                fieldArray[targetX, targetY].secondY = 1;
                                break;
                            case 270:
                                fieldArray[targetX, targetY].firstX = -1;
                                fieldArray[targetX, targetY].secondX = 0;
                                fieldArray[targetX, targetY].firstY = 0;
                                fieldArray[targetX, targetY].secondY = 1;
                                break;
                        }
                        break;
                    case 10:
                        remainedActions++;
                        score--;
                        //if (fieldArray[targetX, targetY].rotation == 0 || fieldArray[targetX, targetY].rotation == 180)
                        //{
                        //    fieldArray[targetX, targetY].firstX = 1;
                        //    fieldArray[targetX, targetY].secondX = -1;
                        //    fieldArray[targetX, targetY].firstY = 0;
                        //    fieldArray[targetX, targetY].secondY = 0;
                        //}
                        //else
                        //{
                        //    fieldArray[targetX, targetY].firstX = 0;
                        //    fieldArray[targetX, targetY].secondX = 0;
                        //    fieldArray[targetX, targetY].firstY = 1;
                        //    fieldArray[targetX, targetY].secondY = -1;
                        //}
                        break;
                }
                remainedActions--;
                score++;
            }
            checkLine();
        } 
    }
}
