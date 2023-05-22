using UnityEngine;
using UnityEngine.UI;
using UniRx;

[RequireComponent(typeof(GridLayoutGroup))]
public class AutoGridLayoutOverrider : MonoBehaviour
{
    [SerializeField] GridLayoutGroup grid = default;

    RectTransform rect = default;

    #region サイズ変動要素の比較用キャッシュ

    int beforeChildCount;

    float beforeWidth, beforeHeight;

    Vector2 beforeCellSize;
    Vector2 beforeSpacing;
    GridLayoutGroup.Axis beforeAxis;
    GridLayoutGroup.Constraint beforeConstraint;
    int beforeConstraingCount;
    #endregion


    void Reset()
    {
        grid = GetComponent<GridLayoutGroup>();
    }

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        while (rect.rect.width == 0 && rect.rect.height == 0)
        {
            rect = rect.transform.parent.GetComponent<RectTransform>();
        }
    }

    void Update()
    {
        ResetLayoutSize();
    }

    void ResetLayoutSize()
    {
        bool skipFlag =
            beforeWidth == rect.rect.width &&
            beforeHeight == rect.rect.height &&
            beforeChildCount == grid.transform.childCount &&
            beforeSpacing.x == grid.spacing.x &&
            beforeSpacing.y == grid.spacing.y &&
            beforeAxis == grid.startAxis &&
            beforeConstraint == grid.constraint;

        switch (grid.constraint)
        {
            case GridLayoutGroup.Constraint.FixedColumnCount:
                skipFlag &= beforeCellSize.y == grid.cellSize.y;
                skipFlag &= beforeConstraingCount == grid.constraintCount;
                break;
            case GridLayoutGroup.Constraint.FixedRowCount:
                skipFlag &= beforeCellSize.x == grid.cellSize.x;
                skipFlag &= beforeConstraingCount == grid.constraintCount;
                break;
        }

        if (skipFlag)
        {
            return;
        }

        float x = grid.cellSize.x;
        float y = grid.cellSize.y;

        switch (grid.constraint)
        {
            case GridLayoutGroup.Constraint.Flexible:
                int childCount = grid.transform.childCount;
                int i = 0;
                while (Mathf.Pow(++i, 2) < childCount) { }

                int tateNum, yokoNum;
                if (Mathf.Pow(i, 2) == childCount)
                {
                    tateNum = yokoNum = i;
                }
                else if (grid.startAxis == GridLayoutGroup.Axis.Horizontal)
                {
                    yokoNum = i;
                    tateNum = childCount / i + (childCount % i != 0 ? 1 : 0);
                }
                else
                {
                    yokoNum = i;
                    if (childCount <= i * (i - 1))
                    {
                        tateNum = childCount / i + (childCount % i != 0 ? 1 : 0);
                    }
                    else
                    {
                        tateNum = i;
                    }
                }

                x = (rect.rect.width - grid.spacing.x * (tateNum - 1)) / tateNum;
                y = (rect.rect.height - grid.spacing.y * (yokoNum - 1)) / yokoNum;
                break;


            case GridLayoutGroup.Constraint.FixedColumnCount:
                x = (rect.rect.width - grid.spacing.x * (grid.constraintCount - 1)) / grid.constraintCount;
                break;


            case GridLayoutGroup.Constraint.FixedRowCount:
                y = (rect.rect.height - grid.spacing.y * (grid.constraintCount - 1)) / grid.constraintCount;
                break;
        }
        beforeCellSize = grid.cellSize = new Vector2(x, y);

        beforeChildCount = grid.transform.childCount;

        beforeWidth = rect.rect.width;
        beforeHeight = rect.rect.height;

        beforeSpacing = grid.spacing;
        beforeAxis = grid.startAxis;
        beforeConstraint = grid.constraint;
        beforeConstraingCount = grid.constraintCount;
    }
}