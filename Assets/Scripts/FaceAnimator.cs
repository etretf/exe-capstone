using System.Net.Sockets;
using UnityEngine;

public class FaceAnimator : MonoBehaviour
{
    public CoderooniConstants CoderooniConstants;
    public SpriteRenderer faceSpriteRenderer;

    public Sprite happy_1;
    public Sprite happy_hearts_2;
    public Sprite happy_uwu_3;
    public Sprite annoyed_4;
    public Sprite dead_5;
    public Sprite shocked_6;
    public Sprite sad_7;
    public Sprite offended_8;
    public Sprite ominous_9;
    public Sprite blank_10;

    public void setFaceSprite(CoderooniConstants.CODEROONI_FACIAL_EXPRESSIONS facialExpression)
    {
        Sprite faceSprite = null;

        switch (facialExpression)
        {
            case CoderooniConstants.CODEROONI_FACIAL_EXPRESSIONS.happy_1:
                faceSprite = happy_1;
                break;
            case CoderooniConstants.CODEROONI_FACIAL_EXPRESSIONS.happy_hearts_2:
                faceSprite = happy_hearts_2;
                break;
            case CoderooniConstants.CODEROONI_FACIAL_EXPRESSIONS.happy_uwu_3:
                faceSprite = happy_uwu_3;
                break;
            case CoderooniConstants.CODEROONI_FACIAL_EXPRESSIONS.annoyed_4:
                faceSprite = annoyed_4;
                break;
            case CoderooniConstants.CODEROONI_FACIAL_EXPRESSIONS.dead_5:
                faceSprite = dead_5;
                break;
            case CoderooniConstants.CODEROONI_FACIAL_EXPRESSIONS.shocked_6:
                faceSprite = shocked_6;
                break;
            case CoderooniConstants.CODEROONI_FACIAL_EXPRESSIONS.sad_7:
                faceSprite = sad_7;
                break;
            case CoderooniConstants.CODEROONI_FACIAL_EXPRESSIONS.offended_8:
                faceSprite = offended_8;
                break;
            case CoderooniConstants.CODEROONI_FACIAL_EXPRESSIONS.ominous_9:
                faceSprite = ominous_9;
                break;
            case CoderooniConstants.CODEROONI_FACIAL_EXPRESSIONS.blank_10:
                faceSprite = blank_10;
                break;
        }

        faceSpriteRenderer.sprite = faceSprite;
    }

    public void clearFacialExpression()
    {
        faceSpriteRenderer.sprite = blank_10;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
