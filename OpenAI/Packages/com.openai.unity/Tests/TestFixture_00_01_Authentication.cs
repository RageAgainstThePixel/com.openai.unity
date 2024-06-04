// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using System;
using System.IO;
using System.Security.Authentication;
using UnityEditor;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_00_01_Authentication
    {
        [SetUp]
        public void Setup()
        {
            var authJson = new OpenAIAuthInfo("sk-test12", "org-testOrg", "proj_testProject");
            var authText = JsonUtility.ToJson(authJson, true);
            File.WriteAllText(OpenAIAuthentication.CONFIG_FILE, authText);
        }

        [Test]
        public void Test_01_GetAuthFromEnv()
        {
            var auth = new OpenAIAuthentication().LoadFromEnvironment();
            Assert.IsNotNull(auth);
            Assert.IsNotNull(auth.Info.ApiKey);
            Assert.IsNotEmpty(auth.Info.ApiKey);
            Assert.IsNotNull(auth.Info.OrganizationId);
            Assert.IsNotEmpty(auth.Info.OrganizationId);
        }

        [Test]
        public void Test_02_GetAuthFromFile()
        {
            var auth = new OpenAIAuthentication().LoadFromPath(Path.GetFullPath(OpenAIAuthentication.CONFIG_FILE));
            Assert.IsNotNull(auth);
            Assert.IsNotNull(auth.Info.ApiKey);
            Assert.AreEqual("sk-test12", auth.Info.ApiKey);
            Assert.IsNotNull(auth.Info.OrganizationId);
            Assert.AreEqual("org-testOrg", auth.Info.OrganizationId);
            Assert.IsNotNull(auth.Info.ProjectId);
            Assert.AreEqual("proj_testProject", auth.Info.ProjectId);
        }

        [Test]
        public void Test_03_GetAuthFromNonExistentFile()
        {
            var auth = new OpenAIAuthentication().LoadFromDirectory(filename: "bad.config");
            Assert.IsNull(auth);
        }

        [Test]
        public void Test_04_GetAuthFromConfiguration()
        {
            var configPath = $"Assets/Resources/{nameof(OpenAIConfiguration)}.asset";
            var cleanup = false;

            if (!File.Exists(Path.GetFullPath(configPath)))
            {
                if (!Directory.Exists($"{Application.dataPath}/Resources"))
                {
                    Directory.CreateDirectory($"{Application.dataPath}/Resources");
                }

                var instance = ScriptableObject.CreateInstance<OpenAIConfiguration>();
                instance.ApiKey = "sk-test12";
                instance.OrganizationId = "org-testOrg";
                AssetDatabase.CreateAsset(instance, configPath);
                cleanup = true;
            }

            var configuration = AssetDatabase.LoadAssetAtPath<OpenAIConfiguration>(configPath);
            var auth = new OpenAIAuthentication(configuration);

            Assert.IsNotNull(auth);
            Assert.IsNotNull(auth.Info.ApiKey);
            Assert.IsNotEmpty(auth.Info.ApiKey);
            Assert.AreEqual(auth.Info.ApiKey, configuration.ApiKey);
            Assert.AreEqual(auth.Info.OrganizationId, configuration.OrganizationId);

            if (cleanup)
            {
                AssetDatabase.DeleteAsset(configPath);
                AssetDatabase.DeleteAsset("Assets/Resources");
            }
        }

        [Test]
        public void Test_05_Authentication()
        {
            var defaultAuth = OpenAIAuthentication.Default = new OpenAIAuthentication().LoadDefault();

            Assert.IsNotNull(defaultAuth);
            Assert.IsNotNull(defaultAuth.Info);
            Assert.IsNotNull(defaultAuth.Info.ApiKey);
            Assert.IsNotNull(defaultAuth.Info.OrganizationId);
            Assert.AreEqual(defaultAuth.Info.ApiKey, OpenAIAuthentication.Default.Info.ApiKey);
            Assert.AreEqual(defaultAuth.Info.OrganizationId, OpenAIAuthentication.Default.Info.OrganizationId);

            var manualAuth = new OpenAIAuthentication("sk-testAA", "org-testAA");
            Assert.IsNotNull(manualAuth);
            Assert.IsNotNull(manualAuth.Info);
            Assert.IsNotNull(manualAuth.Info.ApiKey);
            Assert.IsNotNull(manualAuth.Info.OrganizationId);
            Assert.AreEqual(manualAuth.Info.ApiKey, OpenAIAuthentication.Default.Info.ApiKey);
            Assert.AreEqual(manualAuth.Info.OrganizationId, OpenAIAuthentication.Default.Info.OrganizationId);

            OpenAIAuthentication.Default = defaultAuth;
        }

        [Test]
        public void Test_06_GetKey()
        {
            var auth = new OpenAIAuthentication("sk-testAA");
            Assert.IsNotNull(auth.Info.ApiKey);
            Assert.AreEqual("sk-testAA", auth.Info.ApiKey);
        }

        [Test]
        public void Test_07_GetKeyFailed()
        {
            OpenAIAuthentication auth = null;

            try
            {
                auth = new OpenAIAuthentication("fail-key");
            }
            catch (InvalidCredentialException)
            {
                Assert.IsNull(auth);
            }
            catch (Exception e)
            {
                Assert.IsTrue(false, $"Expected exception {nameof(InvalidCredentialException)} but got {e.GetType().Name}");
            }
        }

        [Test]
        public void Test_08_ParseKey()
        {
            var auth = new OpenAIAuthentication("sk-testAA");
            Assert.IsNotNull(auth.Info.ApiKey);
            Assert.AreEqual("sk-testAA", auth.Info.ApiKey);
            auth = "sk-testCC";
            Assert.IsNotNull(auth.Info.ApiKey);
            Assert.AreEqual("sk-testCC", auth.Info.ApiKey);

            auth = new OpenAIAuthentication("sk-testBB");
            Assert.IsNotNull(auth.Info.ApiKey);
            Assert.AreEqual("sk-testBB", auth.Info.ApiKey);
        }

        [Test]
        public void Test_09_GetOrganization()
        {
            var auth = new OpenAIAuthentication("sk-testAA", "org-testAA");
            Assert.IsNotNull(auth.Info.OrganizationId);
            Assert.AreEqual("org-testAA", auth.Info.OrganizationId);
        }

        [Test]
        public void Test_10_GetOrgFailed()
        {
            OpenAIAuthentication auth = null;

            try
            {
                auth = new OpenAIAuthentication("sk-testAA", "fail-org");
            }
            catch (InvalidCredentialException)
            {
                Assert.IsNull(auth);
            }
            catch (Exception e)
            {
                Assert.IsTrue(false, $"Expected exception {nameof(InvalidCredentialException)} but got {e.GetType().Name}");
            }
        }

        [Test]
        public void Test_11_AzureConfigurationSettings()
        {
            var auth = new OpenAIAuthentication("testKeyAaBbCcDd");
            var settings = new OpenAISettings(resourceName: "test-resource", deploymentId: "deployment-id-test");
            var api = new OpenAIClient(auth, settings);
            Debug.Log(api.Settings.Info.BaseRequest);
            Debug.Log(api.Settings.Info.BaseRequestUrlFormat);
        }

        [Test]
        public void Test_12_CustomDomainConfigurationSettings()
        {
            var auth = new OpenAIAuthentication("sess-customIssuedToken");
            var settings = new OpenAISettings(domain: "OpenAIClient.your-custom-domain.com");
            var api = new OpenAIClient(auth, settings);
            Debug.Log(api.Settings.Info.BaseRequest);
            Debug.Log(api.Settings.Info.BaseRequestUrlFormat);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(OpenAIAuthentication.CONFIG_FILE))
            {
                File.Delete(OpenAIAuthentication.CONFIG_FILE);
            }

            Assert.IsFalse(File.Exists(OpenAIAuthentication.CONFIG_FILE));

            OpenAIAuthentication.Default = null;
            OpenAISettings.Default = null;
        }
    }
}
