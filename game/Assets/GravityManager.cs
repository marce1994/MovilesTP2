using System.Collections.Generic;

public class GravityManager : Singleton<GravityManager>
{
    private List<GravityEntity> nonStaticEntities;
    private List<GravityEntity> staticEntities;

    private void Awake()
    {
        nonStaticEntities = new List<GravityEntity>();
        staticEntities = new List<GravityEntity>();
    }
    private void Update()
    {
        nonStaticEntities.ForEach(x => { x.UpdateGravity(staticEntities); });
    }

    public void AddElement(GravityEntity gravityEntity)
    {
        if(gravityEntity.isStatic)
            staticEntities.Add(gravityEntity);
        else
            nonStaticEntities.Add(gravityEntity);
    }

    public void RemoveElement(GravityEntity gravityEntity)
    {
        if (gravityEntity.isStatic)
            staticEntities.Add(gravityEntity);
        else
            nonStaticEntities.Add(gravityEntity);
    }

    private new void OnDestroy()
    {
        StopAllCoroutines();
        base.OnDestroy();
    }
}
