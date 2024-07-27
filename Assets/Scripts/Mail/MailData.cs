using System;
using System.Collections.Generic;

[Serializable]
public class AddressData
{
    public string street;
    public List<int> numbers;
}

[Serializable]
public class MailData
{
    public List<AddressData> addresses;
    public List<string> names;
    public List<string> surnames;
    public List<string> intros;
    public List<string> middles;
    public List<string> concepts;
    public List<string> finishings;
    public List<int> postmarks;
}
