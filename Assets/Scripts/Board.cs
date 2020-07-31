﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    AbstractPiece[] board = new AbstractPiece[64];

    bool isPieceClicked = false;
    AbstractPiece pieceClicked;
    public GameObject whitePawnPrefab;
    public event Action<Coordinate, Coordinate> OnPieceMove;

    // Start is called before the first frame update
    void Start()
    {
        PawnPiece pawnPiece1 = Instantiate(whitePawnPrefab, new Vector3((float)-3.5, (float)-2.5, 0), Quaternion.identity).GetComponent<PawnPiece>();
        pawnPiece1.Initialize(true, 1, new Coordinate('a', 2));
        board[CoordinateHelper.CoordinateToArrayIndex(pawnPiece1.Position)] = pawnPiece1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Coordinate square = CoordinateHelper.Vector3ToCoordinate(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (isPieceClicked)
            {
                // Update when clicking new tile on board
                if (square != pieceClicked.Position)
                {
                    OnPieceMove?.Invoke(pieceClicked.Position, square); 
                    board[CoordinateHelper.CoordinateToArrayIndex(pieceClicked.Position)] = null;
                    pieceClicked.MoveTo(square);
                    board[CoordinateHelper.CoordinateToArrayIndex(square)] = pieceClicked;
                }
                isPieceClicked = false;
            }
            else if (board[CoordinateHelper.CoordinateToArrayIndex(square)] != null)
            { 
                isPieceClicked = true;
                pieceClicked = board[CoordinateHelper.CoordinateToArrayIndex(square)];
            }
        }
    }

    public void MoveTo(Coordinate src, Coordinate dest)
    {
        AbstractPiece piece = board[CoordinateHelper.CoordinateToArrayIndex(src)];
        if (piece != null)
        {
            board[CoordinateHelper.CoordinateToArrayIndex(src)] = null;
            piece.MoveTo(dest);
            board[CoordinateHelper.CoordinateToArrayIndex(dest)] = piece;
        }
    }
}