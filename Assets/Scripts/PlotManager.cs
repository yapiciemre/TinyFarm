using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    bool isPlanted = false;
    SpriteRenderer plant;

    BoxCollider2D plantCollider;

    int plantStage = 0;
    float timer;

    public Color availableColor = Color.green;
    public Color unavailableColor = Color.red;

    SpriteRenderer plot;

    PlantObject selectedPlant;
    FarmManager farmManager;

    bool isDry = true;
    public Sprite drySprite;
    public Sprite normalSprite;
    public Sprite unavailableSprite;

    float speed = 1f;
    public bool isBought = true;

    void Start()
    {
        plant = transform.GetChild(0).GetComponent<SpriteRenderer>();
        plantCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        farmManager = transform.parent.GetComponent<FarmManager>();
        plot = GetComponent<SpriteRenderer>();
        if (isBought)
        {
            plot.sprite = drySprite;
        }
        else
        {
            plot.sprite = unavailableSprite;
        }
    }

    
    void Update()
    {
        if (isPlanted && !isDry)
        {
            timer -= speed * Time.deltaTime;

            if (timer < 0 && plantStage < selectedPlant.plantStages.Length - 1)
            {
                timer = selectedPlant.timeBtwStages;
                plantStage++;
                UpdatePlant();
            }
        }
    }

    private void OnMouseDown()
    {
        if (isPlanted)
        {
            if (plantStage == selectedPlant.plantStages.Length - 1 && !farmManager.isPlanting && !farmManager.isSelecting)
            {
                Harvest();
            }
        }
        else if (farmManager.isPlanting && farmManager.selectPlant.plant.buyPrice <= farmManager.money && isBought)
        {
            Plant(farmManager.selectPlant.plant);
        }

        if (farmManager.isSelecting)
        {
            switch (farmManager.selectedTool)
            {
                case 1:
                    if (isBought)
                    {
                        isDry = false;
                        plot.sprite = normalSprite;
                        if (isPlanted)
                        {
                            UpdatePlant();
                        }
                    }
                    break;

                case 2:
                    if (farmManager.money >= 10 && isBought)
                    {
                        farmManager.Transaction(-10);
                        if (speed < 2)
                        {
                            speed += .2f;
                        }
                    }
                    break;

                case 3:
                    if (farmManager.money >= 100 && !isBought)
                    {
                        farmManager.Transaction(-100);
                        isBought = true;
                        plot.sprite = drySprite;
                    }
                    break;

                default:
                    break;
            }
        }
    }

    private void OnMouseOver()
    {
        if (farmManager.isPlanting)
        {
            if (isPlanted || farmManager.selectPlant.plant.buyPrice > farmManager.money || !isBought)
            {
                // Can't buy
                plot.color = unavailableColor;

            }
            else
            {
                // Can buy
                plot.color = availableColor;
            }
        }

        if (farmManager.isSelecting)
        {
            switch (farmManager.selectedTool)
            {
                case 1:
                case 2:
                    if (isBought && farmManager.money >= (farmManager.selectedTool -1) * 10)
                    {
                        plot.color = availableColor;
                    }
                    else
                    {
                        plot.color = unavailableColor;
                    }
                    break;

                case 3:
                    if (!isBought && farmManager.money >= 100)
                    {
                        plot.color = availableColor;
                    }
                    else
                    {
                        plot.color = unavailableColor;
                    }
                    break;

                default:
                    plot.color = unavailableColor;
                    break;
            }
        }
    }

    private void OnMouseExit()
    {
        plot.color = Color.white;
    }

    void Harvest()
    {
        isPlanted = false;
        plant.gameObject.SetActive(false);
        farmManager.Transaction(selectedPlant.sellPrice);
        isDry = true;
        plot.sprite = drySprite;
        speed = 1f;
    }

    void Plant(PlantObject newPlant)
    {
        selectedPlant = newPlant;
        isPlanted = true;

        farmManager.Transaction(-selectedPlant.buyPrice);

        plantStage = 0;
        UpdatePlant();
        timer = selectedPlant.timeBtwStages;
        plant.gameObject.SetActive(true);
    }

    void UpdatePlant()
    {
        if (isDry)
        {
            plant.sprite = selectedPlant.dryPlanted;
        }
        else
        {
            plant.sprite = selectedPlant.plantStages[plantStage];
        }

        plantCollider.size = plant.sprite.bounds.size;
        plantCollider.offset = new Vector2 (0, plant.bounds.size.y / 2);
    }
}
