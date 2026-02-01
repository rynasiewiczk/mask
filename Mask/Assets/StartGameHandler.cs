using UnityEngine;

public class StartGameHandler : MonoBehaviour
{
    [SerializeField] private Animation _inputPart;
    [SerializeField] private string _showInputAnim;
    [SerializeField] private Animation _scorePart;
    [SerializeField] private string _showScoreAnim;

    public void ShowGame()
    {
        _inputPart.Play(_showInputAnim);
        _scorePart.Play(_showScoreAnim);
    }
}
