<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="WebCrawler" generation="1" functional="0" release="0" Id="94d6de2f-96c9-4748-9f51-25646907e20d" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="WebCrawlerGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="UserInterface:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/WebCrawler/WebCrawlerGroup/LB:UserInterface:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Indexer:CrawlerStorage" defaultValue="">
          <maps>
            <mapMoniker name="/WebCrawler/WebCrawlerGroup/MapIndexer:CrawlerStorage" />
          </maps>
        </aCS>
        <aCS name="Indexer:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/WebCrawler/WebCrawlerGroup/MapIndexer:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="IndexerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/WebCrawler/WebCrawlerGroup/MapIndexerInstances" />
          </maps>
        </aCS>
        <aCS name="UserInterface:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/WebCrawler/WebCrawlerGroup/MapUserInterface:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="UserInterfaceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/WebCrawler/WebCrawlerGroup/MapUserInterfaceInstances" />
          </maps>
        </aCS>
        <aCS name="WebCrawlerRole:CrawlerStorage" defaultValue="">
          <maps>
            <mapMoniker name="/WebCrawler/WebCrawlerGroup/MapWebCrawlerRole:CrawlerStorage" />
          </maps>
        </aCS>
        <aCS name="WebCrawlerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/WebCrawler/WebCrawlerGroup/MapWebCrawlerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WebCrawlerRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/WebCrawler/WebCrawlerGroup/MapWebCrawlerRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:UserInterface:Endpoint1">
          <toPorts>
            <inPortMoniker name="/WebCrawler/WebCrawlerGroup/UserInterface/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapIndexer:CrawlerStorage" kind="Identity">
          <setting>
            <aCSMoniker name="/WebCrawler/WebCrawlerGroup/Indexer/CrawlerStorage" />
          </setting>
        </map>
        <map name="MapIndexer:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/WebCrawler/WebCrawlerGroup/Indexer/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapIndexerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/WebCrawler/WebCrawlerGroup/IndexerInstances" />
          </setting>
        </map>
        <map name="MapUserInterface:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/WebCrawler/WebCrawlerGroup/UserInterface/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapUserInterfaceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/WebCrawler/WebCrawlerGroup/UserInterfaceInstances" />
          </setting>
        </map>
        <map name="MapWebCrawlerRole:CrawlerStorage" kind="Identity">
          <setting>
            <aCSMoniker name="/WebCrawler/WebCrawlerGroup/WebCrawlerRole/CrawlerStorage" />
          </setting>
        </map>
        <map name="MapWebCrawlerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/WebCrawler/WebCrawlerGroup/WebCrawlerRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWebCrawlerRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/WebCrawler/WebCrawlerGroup/WebCrawlerRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="Indexer" generation="1" functional="0" release="0" software="C:\Users\Nicolas\documents\visual studio 2010\Projects\WebCrawler\WebCrawler\csx\Release\roles\Indexer" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="CrawlerStorage" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Indexer&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Indexer&quot; /&gt;&lt;r name=&quot;UserInterface&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;WebCrawlerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/WebCrawler/WebCrawlerGroup/IndexerInstances" />
            <sCSPolicyFaultDomainMoniker name="/WebCrawler/WebCrawlerGroup/IndexerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="UserInterface" generation="1" functional="0" release="0" software="C:\Users\Nicolas\documents\visual studio 2010\Projects\WebCrawler\WebCrawler\csx\Release\roles\UserInterface" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;UserInterface&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Indexer&quot; /&gt;&lt;r name=&quot;UserInterface&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;WebCrawlerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/WebCrawler/WebCrawlerGroup/UserInterfaceInstances" />
            <sCSPolicyFaultDomainMoniker name="/WebCrawler/WebCrawlerGroup/UserInterfaceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="WebCrawlerRole" generation="1" functional="0" release="0" software="C:\Users\Nicolas\documents\visual studio 2010\Projects\WebCrawler\WebCrawler\csx\Release\roles\WebCrawlerRole" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="CrawlerStorage" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WebCrawlerRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Indexer&quot; /&gt;&lt;r name=&quot;UserInterface&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;WebCrawlerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/WebCrawler/WebCrawlerGroup/WebCrawlerRoleInstances" />
            <sCSPolicyFaultDomainMoniker name="/WebCrawler/WebCrawlerGroup/WebCrawlerRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="IndexerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="UserInterfaceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="WebCrawlerRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="IndexerInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="UserInterfaceInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="WebCrawlerRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="48bb1043-2a4d-4e22-b8b7-ea5a62dd689c" ref="Microsoft.RedDog.Contract\ServiceContract\WebCrawlerContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="c929019e-1bd7-4fd4-82c7-1bc0db0522d5" ref="Microsoft.RedDog.Contract\Interface\UserInterface:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/WebCrawler/WebCrawlerGroup/UserInterface:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>